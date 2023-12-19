
// Function to convert XML Document to JavaScript object
function xmlToJson(xml) {
    let obj = {};
    if (xml.nodeType == 1) {                
        if (xml.attributes.length > 0) {
            obj["@attributes"] = {};
            for (let j = 0; j < xml.attributes.length; j++) {
                let attribute = xml.attributes.item(j);
                obj["@attributes"][attribute.nodeName] = attribute.nodeValue;
            }
        }
    } else if (xml.nodeType == 3) { 
        obj = xml.nodeValue;
    }            

    if (xml.hasChildNodes()) {
        for(let i = 0; i < xml.childNodes.length; i++) {
            let item = xml.childNodes.item(i);
            let nodeName = item.nodeName;
            if (typeof(obj[nodeName]) == "undefined") {
                obj[nodeName] = xmlToJson(item);
            } else {
                if (typeof(obj[nodeName].push) == "undefined") {
                    let old = obj[nodeName];
                    obj[nodeName] = [];
                    obj[nodeName].push(old);
                }
                obj[nodeName].push(xmlToJson(item));
            }
        }
    }

    if (typeof obj === 'object' && Object.keys(obj).length === 1 && '#text' in obj) {
        obj = obj['#text'];
    }

    return cleanObject(obj);
};

function cleanObject(obj) {
    let cleanObj = Array.isArray(obj) ? [] : {};

    for (let key in obj) {
        if (key !== '@attributes' && key !== '#text') {
            if (typeof obj[key] === 'object' && obj[key] !== null) {
                cleanObj[key] = cleanObject(obj[key]);
            } else {
                cleanObj[key] = obj[key];
            }
        }
    }

    return cleanObj;
}

export { xmlToJson };