import convert from 'xml-js';

// Function to convert XML Document to JavaScript object
function xmlToJson(xml) {
    if (!xml) return null;

    const convertedObject = convert.xml2js(xml, { compact: true, ignoreAttributes: true, ignoreDeclaration: true })
    console.log('convertedObject', convertedObject);
    return cleanUp(convertedObject);
};

function cleanUp(convertedObject) {
    if (!convertedObject) return null;

    if (Array.isArray(convertedObject)) {
        return convertedObject.map(item => {
            if (typeof item === 'object' && item !== null && "_text" in item) {
                return item["_text"];
            } else {
                return cleanUp(item);
            }
        });
    }

    let cleanedObject = {};

    for (let key in convertedObject) {
        if (convertedObject.hasOwnProperty(key)) {
            let value = convertedObject[key];
            if (typeof value === 'object' && value !== null) {
                if ("_text" in value) {
                    cleanedObject[key] = value["_text"];
                } else if (Object.keys(value).length > 0) {
                    cleanedObject[key] = cleanUp(value);
                }
            } else {
                cleanedObject[key] = value;
            }
        }
    }

    return cleanedObject;
}

export { xmlToJson };