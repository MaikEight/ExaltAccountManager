
import TestWidget from './Widgets/TestWidget';
import StatsWidget from './Widgets/StatsWidget';
import ActivityWidget from './Widgets/ActivityWidget';
import QuickActionsWidget from './Widgets/QuickActionsWidget';
import EditOutlinedIcon from '@mui/icons-material/EditOutlined';
import BasicActionsWidget from './Widgets/BasicActionsWidget';
import AutoAwesomeMosaicOutlinedIconRotate from '../Icons/AutoAwesomeMosaicOutlinedIconRotate';
import BestCharactersWidget from './Widgets/BestCharactersWidget';
import AuditLogWidget from './Widgets/AuditLogWidget';
import TableDataWidget from './Widgets/TableDataWidget';

export class Widgets {
    static getWidgetByType(type) {
        switch (type) {
            case Widgets.BASIC_ACTIONS.type:
                return Widgets.BASIC_ACTIONS;
            case Widgets.TEST.type:
                return Widgets.TEST;
            case Widgets.STATS.type:
                return Widgets.STATS;
            case Widgets.ACTIVITY.type:
                return Widgets.ACTIVITY;
            case Widgets.QUICK_ACTIONS.type:
                return Widgets.QUICK_ACTIONS;
            case Widgets.BEST_CHARACTERS.type:
                return Widgets.BEST_CHARACTERS;
            case Widgets.AUDITLOG.type:
                return Widgets.AUDITLOG;
            case Widgets.ACCOUNT_DETAILS.type:
                return Widgets.ACCOUNT_DETAILS;
            default:
                return null;
        }
    }

    static BASIC_ACTIONS = {
        type: 'BASIC_ACTIONS',
        name: 'Actions',
        icon: AutoAwesomeMosaicOutlinedIconRotate,
        Component: BasicActionsWidget,
        minSlots: 1,
        maxSlots: 2,
        defaultConfig: {
            slots: 2,
            settings: {

            }
        }
    }

    static BEST_CHARACTERS = {
        type: 'BEST_CHARACTERS',
        name: 'Best Characters',
        Component: BestCharactersWidget,
        minSlots: 1,
        maxSlots: 2,
        defaultConfig: {
            slots: 1,
            settings: {

            }
        }
    }

    static AUDITLOG = {
        type: 'AUDITLOG',
        name: 'Audit Log',
        Component: AuditLogWidget,
        minSlots: 1,
        maxSlots: 2,
        defaultConfig: {
            slots: 1,
            settings: {
                
            }
        }
    }

    static ACCOUNT_DETAILS = {
        type: 'ACCOUNT_DETAILS',
        name: 'Account Details',
        Component: TableDataWidget,
        dataSourceObject: null, // to be used for sub-data sources
        editFunctionType: 'ACCOUNT',
        minSlots: 1,
        maxSlots: 2,
        defaultConfig: {
            slots: 1,
            settings: {                
                fields: {
                    email: {
                        dataField: 'email',
                        displayName: 'Email',
                        visible: true,
                        editable: false,
                        dataType: 'string',
                        showCopyButton: true,
                    },
                    steamId: {
                        dataField: 'steamId',
                        displayName: 'Steam ID',
                        visible: false,
                        editable: false,
                        dataType: 'string',
                        showCopyButton: true,
                    },
                    name: {
                        dataField: 'name',
                        displayName: 'Name',
                        visible: true,
                        editable: false,
                        dataType: 'string',
                        showCopyButton: true,
                    },
                    password: {
                        dataField: 'password',
                        displayName: 'Password',
                        visible: true,
                        editable: true,
                        dataType: 'password',
                        showCopyButton: false,
                    },
                    performDailyLogin: {
                        dataField: 'performDailyLogin',
                        displayName: 'Daily Login',
                        visible: true,
                        editable: true,
                        dataType: 'boolean',
                        showCopyButton: false,
                    },
                    lastLogin: {
                        dataField: 'lastLogin',
                        displayName: 'Last Login',
                        visible: true,
                        editable: false,
                        dataType: 'datetime',
                        showCopyButton: false,
                    },
                    lastRefresh: {
                        dataField: 'lastRefresh',
                        displayName: 'Last Refresh',
                        visible: false,
                        editable: false,
                        dataType: 'datetime',
                        showCopyButton: false,
                    },
                    serverName: {
                        dataField: 'serverName',
                        displayName: 'Server',
                        visible: false,
                        editable: false,
                        dataType: 'server',
                        showCopyButton: false,
                    },
                    state: {
                        dataField: 'state',
                        displayName: 'State',
                        visible: false,
                        editable: false,
                        dataType: 'state',
                        showCopyButton: false,
                    },
                    orderId: {
                        dataField: 'orderId',
                        displayName: 'Order ID',
                        visible: false,
                        editable: false,
                        dataType: 'string',
                        showCopyButton: false,
                    },
                }
            }
        }
    }

    static TEST = {
        type: 'TEST',
        name: 'Test Widget',
        icon: EditOutlinedIcon,
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