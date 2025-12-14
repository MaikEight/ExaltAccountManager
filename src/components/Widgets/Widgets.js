
import TestWidget from './Widgets/TestWidget';
import StatsWidget from './Widgets/StatsWidget';
import ActivityWidget from './Widgets/ActivityWidget';
import QuickActionsWidget from './Widgets/QuickActionsWidget';

export class Widgets {
    static getWidgetByType(type) {
        switch (type) {
            case Widgets.TEST.type:
                return Widgets.TEST;
            case Widgets.STATS.type:
                return Widgets.STATS;
            case Widgets.ACTIVITY.type:
                return Widgets.ACTIVITY;
            case Widgets.QUICK_ACTIONS.type:
                return Widgets.QUICK_ACTIONS;
            default:
                return null;
        }
    }

    static TEST = {
        type: 'TEST',
        name: 'Test Widget',
        Component: TestWidget,
        minSlots: 1, // Minimum slots this widget can occupy
        maxSlots: 2, // Maximum slots this widget can occupy
        defaultConfig: {
            slots: 1, // How many slots this widget currently occupies
            settings: {}
        }
    }

    static STATS = {
        type: 'STATS',
        name: 'Account Statistics',
        Component: StatsWidget,
        minSlots: 1,
        maxSlots: 1,
        defaultConfig: {
            slots: 1,
            settings: {}
        }
    }

    static ACTIVITY = {
        type: 'ACTIVITY',
        name: 'Activity Monitor',
        Component: ActivityWidget,
        minSlots: 1,
        maxSlots: 2,
        defaultConfig: {
            slots: 1,
            settings: {}
        }
    }

    static QUICK_ACTIONS = {
        type: 'QUICK_ACTIONS',
        name: 'Quick Actions',
        Component: QuickActionsWidget,
        minSlots: 1,
        maxSlots: 2,
        defaultConfig: {
            slots: 1,
            settings: {}
        }
    }
}