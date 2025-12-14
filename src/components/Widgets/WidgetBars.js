import AccountHeader from "./WidgetBars/Components/AccountHeader";
import WidgetBarBase from './WidgetBars/WidgetBarBase';

export class WidgetBars {

    // Utility function to get widget bar by type
    static getWidgetBarByType(type) {
        switch (type) {
            case WidgetBars.ACCOUNT.type:
                return WidgetBars.ACCOUNT;
            default:
                return null;
        }
    }

    //Utility function to get widget bar width by slots
    static getWidgetBarWidthBySlots(slots) {
        const baseWidth = 450; // Base width for 1 slot
        const paddingInPixels = 16; // Padding in pixels per space (left, right, between)
        return (slots * baseWidth) + ((slots - 1) * paddingInPixels);
    }

    // TYPES
    static ACCOUNT = {
        type: 'ACCOUNT',
        name: 'Account',
        maxSlots: 2,
        BarComponent: WidgetBarBase,
        headerComponents: [
            AccountHeader
        ],
        components: [],
        footerComponents: [],
        defaultConfig: {
            slots: 1,
            // activeWidgets: []
        }
    }
}

export class WidgetBarEvents {
    static ON_OPEN = 'onOpen';
    static ON_CLOSE = 'onClose';

    static validateName(name) {
        return [
            WidgetBarEvents.ON_OPEN,
            WidgetBarEvents.ON_CLOSE,
        ].includes(name);
    }
}