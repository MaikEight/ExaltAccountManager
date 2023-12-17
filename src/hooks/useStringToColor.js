import hashString from 'hash-string';
import useColorList from './useColorList';

const useStringToColor = (inputString) => {    
    const colorList = useColorList();
    const colorMap = { Default: 0 };

    if(typeof(colorMap[inputString]) !== 'undefined') return colorList[colorMap[inputString]];
    
    const generateColorIndex = (name) => {
        const hash = hashString(name);
        const hashInteger = Math.abs(parseInt(hash, 16));
        return hashInteger % colorList.length;
    };

    const index = generateColorIndex(inputString);
    colorMap[inputString] = index;
    return colorList[index];
};

export default useStringToColor;
