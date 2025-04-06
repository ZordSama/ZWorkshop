const fs = require('fs');
const path = require('path');

const indexPath = path.join(__dirname, '../.output/public/index.html');

fs.readFile(indexPath, 'utf8', (err, data) => {
  if (err) {
    console.error('Failed to read index.html:', err);
    return;
  }

  const updatedData = data
    .replace(/href="\/_nuxt/g, 'href="./_nuxt')
    .replace(/href="\/assets/g, 'href="./assets') // Add this if you have other assets
    .replace(/src="\/_nuxt/g, 'src="./_nuxt') // Also replace src attributes if needed
    .replace(/src="\/assets/g, 'src="./assets');

  fs.writeFile(indexPath, updatedData, 'utf8', (err) => {
    if (err) {
      console.error('Failed to write updated index.html:', err);
    } else {
      console.log('Successfully updated paths in index.html');
    }
  });
});