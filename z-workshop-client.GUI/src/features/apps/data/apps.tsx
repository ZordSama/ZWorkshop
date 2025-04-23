import fs from 'fs'
import { IAppCapsule } from '@/utils/types'

const readAppsCapsuleData = (filepath: string): IAppCapsule[] => {
  try {
    const data = fs.readFileSync(filepath, 'utf8')
    const rawJsonData = JSON.parse(data)

    if (!Array.isArray(rawJsonData)) {
      throw new Error('JSON data is not an array')
    }
    const appsCapsulesData: IAppCapsule[] = rawJsonData.map(
      (appCapsule: any) => ({
        id: appCapsule.app_id,
        name: appCapsule.app_name,
        thumbnail: `${process.env.PUBLIC_URL}/product_thumbnails/${appCapsule.filename}`,
        fallback: appCapsule.url,
        price: Math.random() * 100,
      })
    )
    return appsCapsulesData
  } catch (ex) {
    if (ex instanceof Error) {
      console.error('Caught an error:', ex.message)
    } else {
      console.error('Unknown error:', ex)
    }
    return [] as IAppCapsule[]
  }
}
export const appCapsules = readAppsCapsuleData(
  './apps.json'
) as Array<IAppCapsule>
