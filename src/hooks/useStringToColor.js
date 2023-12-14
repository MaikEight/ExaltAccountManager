import hashString from 'hash-string';
import useColorList from './useColorList';

const useStringToColor = (inputString) => {    
    const colorList = useColorList();

    const generateColorIndex = (name) => {
        const hash = hashString(name);
        const hashInteger = Math.abs(parseInt(hash, 16));
        return hashInteger % colorList.length;
    };

    const index = generateColorIndex(inputString);
    return colorList[index];
};

export default useStringToColor;
