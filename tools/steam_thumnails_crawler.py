import time
import requests
import os
import random
import json
from selenium import webdriver
from selenium.webdriver.chrome.service import Service
from selenium.webdriver.chrome.options import Options
from selenium.webdriver.common.by import By
from selenium.webdriver.common.keys import Keys
from selenium.webdriver.common.action_chains import ActionChains
from selenium.webdriver.support.wait import WebDriverWait
from webdriver_manager.chrome import ChromeDriverManager
from bs4 import BeautifulSoup
from urllib.parse import urlparse
import re
from urllib.parse import unquote

MAX_IMAGES = 300
MIN_IMAGE_SIZE = 5000 

def sanitize_filename(input_string):
    invalid_chars = r'[<>:"/\\|?*\x00-\x1F]'
    invalid_end_chars = r"[. ]+$"
    sanitized_string = re.sub(invalid_chars, "", input_string.strip().lower())
    sanitized_string = re.sub(invalid_end_chars, "", sanitized_string)
    sanitized_string = re.sub(r"\s+", "_", sanitized_string)
    return sanitized_string


def extract_background_url(style):
    match = re.search(r'url\(["\']?(.*?)["\']?\)', style)
    return match.group(1) if match else None


def is_valid_image(img_url):
    # Skip unwanted formats and placeholder images
    if any(img_url.lower().endswith(ext) for ext in (".gif", ".svg", ".png")):
        return False
    if "transparent" in img_url.lower() or "blank" in img_url.lower():
        return False
    return True


# def find_app_name(element, soup):
#     # First try: look for h1 in parent home_content
#     parent = element.find_parent('div', class_='home_content')
#     if parent:
#         h1 = parent.find('h1')
#         if h1:
#             return h1.get_text(strip=True)

#     # Second try: look in nearby elements for app names
#     for ancestor in element.parents:
#         if ancestor.find('h1'):
#             return ancestor.find('h1').get_text(strip=True)
#         if ancestor.find('div', class_='app_name'):
#             return ancestor.find('div', class_='app_name').get_text(strip=True)

#     # Final fallback: look for title in meta tags
#     meta_title = soup.find('meta', property='og:title')
#     if meta_title:
#         return meta_title.get('content', '').split(' on Steam')[0]

#     return ''


def extract_app_data(element):
    """Extracts app metadata from parent links and DOM elements"""
    app_id = None
    app_name = None

    # Look for parent app links
    parent_a = element.find_parent("a", href=re.compile(r"/app/\d+"))
    if parent_a:
        href = parent_a.get("href", "")

        # Extract app ID and name from URL pattern: /app/{id}/{name}/
        url_match = re.search(r"/app/(\d+)/([^/]+)/?", href)
        if url_match:
            app_id = url_match.group(1)
            # Decode URL-encoded characters and sanitize
            app_name = sanitize_filename(unquote(url_match.group(2)))

        # If name not in URL, try to find in DOM
        if not app_name:
            container = element.find_parent(class_=re.compile("content|browse|game"))
            if container:
                name_element = container.find(class_=re.compile("title|name|appname"))
                if name_element:
                    app_name = sanitize_filename(name_element.get_text(strip=True))

    return app_id, app_name


def generate_filename(img_url, app_id, app_name, index):
    """Generates filename using app metadata with URL fallback"""
    # Get base name from URL as fallback
    path = unquote(urlparse(img_url).path)
    url_base = os.path.splitext(os.path.basename(path))[0]

    name_parts = []
    if app_name:
        name_parts.append(app_name)
    if app_id:
        name_parts.append(f"appid_{app_id}")
    name_parts.append(url_base)

    # Remove duplicate parts (e.g., if app_name matches url_base)
    unique_parts = []
    seen = set()
    for part in name_parts:
        clean_part = part.lower()
        if clean_part not in seen:
            seen.add(clean_part)
            unique_parts.append(part)

    return f"{'_'.join(unique_parts)}_{index}.jpg"


if not os.path.exists("steam_thumbnails"):
    os.makedirs("steam_thumbnails")

try:
    chrome_options = Options()
    user_data_dir = "C:/Users/zord/AppData/Local/Google/Chrome/User Data"
    profile_dir = "Default"
    chrome_options.add_argument(f"--user-data-dir={user_data_dir}")
    chrome_options.add_argument(f"--profile-directory={profile_dir}")
    chrome_options.add_argument("--disable-infobars")

    driver = webdriver.Chrome(
        service=Service(ChromeDriverManager().install()), options=chrome_options
    )
    driver.set_page_load_timeout(60)

    print("Opening Steam homepage...")
    driver.get("https://store.steampowered.com/")

    # Age gate handling
    try:
        WebDriverWait(driver, 5).until(
            EC.presence_of_element_located((By.ID, "ageYear"))
        )
        driver.find_element(By.ID, "ageYear").send_keys("1985")
        driver.find_element(By.ID, "view_product_page_btn").click()
        print("Dismissed age gate")
    except:
        pass

    # Improved scrolling
    last_height = driver.execute_script("return document.body.scrollHeight")
    for _ in range(10):
        driver.execute_script("window.scrollTo(0, document.body.scrollHeight);")
        time.sleep(2)
        new_height = driver.execute_script("return document.body.scrollHeight")
        if new_height == last_height:
            break
        last_height = new_height

    print("Extracting content...")
    soup = BeautifulSoup(driver.page_source, "html.parser")
    all_images = []

    # 1. Regular capsule images
    all_images += soup.find_all("img", class_="capsule")

    # 2. Images inside capsule containers
    capsule_containers = soup.find_all(class_=re.compile("capsule"))
    for container in capsule_containers:
        if container.find("img"):
            all_images.append(container.find("img"))

    # 3. Spotlight images (filter out non-game thumbnails)
    spotlight_divs = soup.find_all("div", class_="spotlight_img")
    for spotlight in spotlight_divs:
        img = spotlight.find("img")
        if img and "header" not in img["src"]:
            all_images.append(img)

    # 4. Background capsule images
    bg_containers = soup.find_all(class_=re.compile("capsule"))
    for container in bg_containers:
        if container.has_attr("data-background-image-url"):
            img_url = container["data-background-image-url"]
            fake_img = soup.new_tag("img")
            fake_img["src"] = img_url
            all_images.append(fake_img)
        elif container.has_attr("style"):
            bg_url = extract_background_url(container["style"])
            if bg_url:
                fake_img = soup.new_tag("img")
                fake_img["src"] = bg_url
                all_images.append(fake_img)

    # Deduplicate
    seen_urls = set()
    unique_images = []
    for img in all_images:
        img_url = img.get("src", "")
        if not img_url:
            continue

        if img_url.startswith("//"):
            img_url = f"https:{img_url}"

        if img_url not in seen_urls:
            seen_urls.add(img_url)
            unique_images.append(img)

    game_thumbnails = unique_images[:MAX_IMAGES]
    print(f"Found {len(game_thumbnails)} unique thumbnails")

    # Download images
    image_data = []
    for idx, img in enumerate(game_thumbnails):
        img_url = img.get('src', '')
        if not img_url or not is_valid_image(img_url):
            continue

        # Extract app metadata from parent elements
        app_id, app_name = extract_app_data(img)
        
        try:
            # Get image metadata without downloading
            head_resp = requests.head(img_url, timeout=10)
            if int(head_resp.headers.get('Content-Length', 0)) < MIN_IMAGE_SIZE:
                print(f"Skipping small image: {img_url}")
                continue
                
            # Generate meaningful filename
            filename = generate_filename(img_url, app_id, app_name, idx)
            filepath = os.path.join("steam_thumbnails", filename)
            
            # Download image
            response = requests.get(img_url, stream=True, timeout=15)
            if response.status_code == 200:
                with open(filepath, 'wb') as f:
                    for chunk in response.iter_content(1024):
                        f.write(chunk)
                
                image_data.append({
                    "filename": filename,
                    "app_id": app_id,
                    "app_name": app_name,
                    "url": img_url,
                    "source": img.parent.name if img.parent else 'unknown'
                })
                print(f"Downloaded {idx+1}/{len(game_thumbnails)}: {filename}")

        except Exception as e:
            print(f"Failed to download {img_url}: {str(e)}")

    # Save metadata
    with open(os.path.join("steam_thumbnails", "metadata.json"), "w") as f:
        json.dump(image_data, f, indent=2)

except Exception as e:
    print(f"Error: {str(e)}")
    import traceback

    traceback.print_exc()

finally:
    try:
        driver.quit()
    except:
        pass
    print("Script finished")
