import { useEffect, useState } from "react";
import WidgetBase from "./WidgetBase";
import useApplySettingsToHeaderName from "../../../hooks/useApplySettingsToHeaderName";
import useWidgets from "../../../hooks/useWidgets";
import { formatTime, getAuditLogForAccount } from 'eam-commons-js';
import { Box } from "@mui/material";
import { DataGrid } from "@mui/x-data-grid";
import { WidgetBars } from "../Widgetbars";

function AuditLogWidget({ type, widgetId }) {
    const { applySettingsToHeaderName } = useApplySettingsToHeaderName();
    const { getWidgetConfiguration, widgetBarState, widgetBarConfig } = useWidgets();

    const config = getWidgetConfiguration(type);
    const slotSize = Math.min((widgetBarConfig?.slots || 1), (config?.slots || type?.defaultConfig?.slots || 1));

    const [auditLogs, setAuditLogs] = useState([]);
    const [hiddenColumns, setHiddenColumns] = useState({ accountEmail: false });

    const auditLogColumns = [
        { field: 'id', headerName: applySettingsToHeaderName('🆔 ID'), width: 65 },
        { field: 'time', headerName: applySettingsToHeaderName('🕑 Time'), width: 130, type: 'dateTime', renderCell: (params) => <div style={{ textAlign: 'center' }}> {formatTime(params.value)} </div> },
        { field: 'sender', headerName: applySettingsToHeaderName('👤 Sender'), width: 100 },
        { field: 'message', headerName: applySettingsToHeaderName('💬 Message'),  minWidth: 150, flex: 0.5 },
        { field: 'accountEmail', headerName: applySettingsToHeaderName('📧 Email'), flex: 0.325 }
    ];

    useEffect(() => {
        const columnsToHide = {};
        columnsToHide.accountEmail = false;

        if (slotSize === 1) {
            columnsToHide.id = false;
            columnsToHide.sender = false;
        }
        setHiddenColumns(columnsToHide);
    }, [widgetBarConfig.slots, config.slots]);

    useEffect(() => {
        if (!widgetBarState?.data || !widgetBarState.data.email) {
            setAuditLogs([]);
            return;
        }
        getAuditLogForAccount(widgetBarState.data.email)
            .then((logs) => {
                const auditLog = logs.map((log) => {
                    return {
                        ...log,
                        time: new Date(log.time),
                    }
                }).sort((a, b) => b.id - a.id);

                setAuditLogs(auditLog);
            })
            .catch((err) => {
                console.error("Error fetching audit logs:", err);
                setAuditLogs([]);
            });

    }, [widgetBarState.data]);

    return (
        <WidgetBase type={type} widgetId={widgetId}>
            <Box
                sx={{
                    width: '100%',
                    minWidth: '300px',
                    maxWidth: `${(slotSize === 1 ? (widgetBarConfig?.slots || 1) === 1 ? WidgetBars.WIDGET_SLOT_WIDTH - 16 : WidgetBars.WIDGET_SLOT_WIDTH - 16 : (WidgetBars.WIDGET_SLOT_WIDTH * 2))}px`,
                    minHeight: '150px',
                }}
            >
                {/* Audit Log Table */}
                <DataGrid
                    sx={{
                        maxHeight: '300px',
                        borderRadius: (theme) => `${(theme.shape.borderRadius * 2) - 6}px`,
                    }}
                    initialState={{
                        columns: {
                            columnVisibilityModel: { ...hiddenColumns },
                        },
                    }}
                    columnVisibilityModel={hiddenColumns}
                    rows={auditLogs}
                    columns={auditLogColumns}
                    pageSize={10}
                    rowsPerPageOptions={[10, 25, 50, 100]}
                    rowSelection={false}
                />
            </Box>
        </WidgetBase>
    );
}

export default AuditLogWidget;