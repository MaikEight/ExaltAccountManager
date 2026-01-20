import BasicActionsWidget from './Widgets/BasicActionsWidget';
import AutoAwesomeMosaicOutlinedIconRotate from '../Icons/AutoAwesomeMosaicOutlinedIconRotate';
import BestCharactersWidget from './Widgets/BestCharactersWidget';
import AuditLogWidget from './Widgets/AuditLogWidget';
import TableDataWidget from './Widgets/TableDataWidget';
import SingleCharacterOverviewWidget from './Widgets/SingleCharacterOverviewWidget';
import PersonOutlineOutlinedIcon from '@mui/icons-material/PersonOutlineOutlined';
import GroupsOutlinedIcon from '@mui/icons-material/GroupsOutlined';
import HistoryEduRoundedIcon from '@mui/icons-material/HistoryEduRounded';
import TocRoundedIcon from '@mui/icons-material/TocRounded';

export class Widgets {
    static getWidgetByType(type) {
        switch (type) {
            case Widgets.BASIC_ACTIONS.type:
                return Widgets.BASIC_ACTIONS;
            case Widgets.BEST_CHARACTERS.type:
                return Widgets.BEST_CHARACTERS;
            case Widgets.SINGLE_CHARACTER_OVERVIEW.type:
                return Widgets.SINGLE_CHARACTER_OVERVIEW;
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
        icon: GroupsOutlinedIcon,
        Component: BestCharactersWidget,
        minSlots: 1,
        maxSlots: 2,
        defaultConfig: {
            slots: 1,
            settings: {

            }
        }
    }

    static SINGLE_CHARACTER_OVERVIEW = {
        type: 'SINGLE_CHARACTER_OVERVIEW',
        name: 'Single Character',
        icon: PersonOutlineOutlinedIcon,
        Component: SingleCharacterOverviewWidget, 
        minSlots: 1,
        maxSlots: 2,
        defaultConfig: {
            slots: 1,
            daysToFetchOptions: [3, 7, 14, 30],
            settings: {
                daysToFetch: 7,
                characterIdPerAccount: {}, // Mapping of account email to character ID                
            }
        }
    };

    static AUDITLOG = {
        type: 'AUDITLOG',
        name: 'Audit Log',
        icon: HistoryEduRoundedIcon,
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
        icon: TocRoundedIcon,
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
}