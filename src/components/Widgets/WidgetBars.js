import AccountHeader from "./WidgetBars/Components/AccountHeader";
import WidgetGrid from "./WidgetBars/Components/WidgetGrid";
import WidgetControls from "./WidgetBars/Components/WidgetControls";
import WidgetBarBase from './WidgetBars/WidgetBarBase';
import { Widgets } from './Widgets';

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

    static WIDGET_SLOT_WIDTH = 450; // Width in pixels for a single widget slot

    //Utility function to get widget bar width by slots
    static getWidgetBarWidthBySlots(slots) {
        const paddingInPixels = 8; // Padding in pixels per space (left, right, between)
        return (slots * WidgetBars.WIDGET_SLOT_WIDTH) + ((slots  + 1) * paddingInPixels);
    }

    // TYPES
    static ACCOUNT = {
        type: 'ACCOUNT',
        name: 'Account',
        maxSlots: 2,
        BarComponent: WidgetBarBase,
        availableWidgets: [
            Widgets.ACCOUNT_DETAILS,
            Widgets.BASIC_ACTIONS,
            Widgets.BEST_CHARACTERS,
            Widgets.SINGLE_CHARACTER_OVERVIEW,
            Widgets.AUDITLOG,
        ],
        headerComponents: [
            AccountHeader
        ],
        components: [
            WidgetGrid
        ],
        footerComponents: [],
        floatingComponents: [
            WidgetControls,
        ],
        defaultConfig: {
            slots: 1,
            activeWidgets: []
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