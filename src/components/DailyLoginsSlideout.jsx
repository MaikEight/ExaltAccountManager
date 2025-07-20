
import { useTheme } from "@emotion/react";
import { Box, Drawer, IconButton, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography } from "@mui/material";
import { useEffect, useRef, useState } from "react";
import CloseIcon from '@mui/icons-material/Close';
import { invoke } from "@tauri-apps/api/core";
import ComponentBox from "./ComponentBox";
import PaddedTableCell from "./AccountDetails/PaddedTableCell";
import TextTableRow from "./AccountDetails/TextTableRow";
import { formatTime } from "eam-commons-js";
import CheckCircleOutlinedIcon from '@mui/icons-material/CheckCircleOutlined';
import CancelOutlinedIcon from '@mui/icons-material/CancelOutlined';
import ArticleOutlinedIcon from '@mui/icons-material/ArticleOutlined';
import { useNavigate } from "react-router-dom";

const entryStatus = [
    "Succeeded",
    "Processing",
    "Failed"
];

function DailyLoginsSlideout({ isOpen, report, onClose }) {
    const [reportData, setReportData] = useState({
        succeeded: [],
        failed: [],
    });
    const [reportDuration, setReportDuration] = useState('');
    const [isUtcZero, setIsUtcZero] = useState(false);
    const containerRef = useRef(null);
    const theme = useTheme();

    useEffect(() => {
        if (!report) {
            setReportData({ succeeded: [], failed: [] });
            setReportDuration('');
            setIsUtcZero(false);
            return;
        }
        setIsUtcZero(new Date(report.endTime).getUTCHours() === 0
            && new Date(report.endTime).getUTCMinutes() === 0
            && new Date(report.endTime).getUTCSeconds() === 0
        );

        invoke('get_daily_login_report_entries_by_report_id', { reportId: report.id })
            .then((data) => {
                const entries = data.sort((a, b) => {
                    return entryStatus.indexOf(a.status) - entryStatus.indexOf(b.status);
                });

                setReportData({
                    succeeded: entries.filter((entry) => entry.status === 'Succeeded'),
                    failed: entries.filter((entry) => entry.status === 'Failed'),
                });
            });

        setReportDuration(getReportDuration());
    }, [report]);

    const getReportDuration = () => {
        if (!report) return '';
        if (!report.startTime || !report.endTime) return '';
        if (report.endTime < report.startTime) return '';
        if (report.endTime === report.startTime) return '0s';
        if (report.hasFinished === false) return new Date(report.startTime).setHours(0, 0, 0, 0) === new Date().setHours(0, 0, 0, 0) ? 'still running' : '';

        const duration = (report.endTime - report.startTime) / 1000;
        const hours = Math.floor(duration / 3600);
        const minutes = Math.floor((duration % 3600) / 60);
        const seconds = Math.floor(duration % 60);
        return `${hours}h ${minutes}m ${seconds}s`;
    };

    return (
        <Box ref={containerRef} sx={{ overflow: 'hidden' }}>
            <Drawer
                sx={{
                    width: 500,
                    flexShrink: 0,
                    '& .MuiDrawer-paper': {
                        width: 500,
                        backgroundColor: theme.palette.background.paperLight,
                        borderRadius: `${theme.shape.borderRadius}px 0 0 ${theme.shape.borderRadius}px`,
                        overflow: 'hidden',
                    },
                }}
                slotProps={{
                    paper: {
                        elevation: 0, 
                        square: false, 
                        sx: { 
                            borderRadius: `${theme.shape.borderRadius}px 10px 10px ${theme.shape.borderRadius}px`, 
                            overflow: 'hidden' 
                        }
                    },
                    transition: {
                        container: containerRef.current
                    }
                }}
                variant="persistent"
                anchor="right"
                open={isOpen}
            >
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'row',
                        justifyContent: 'center',
                        alignContent: 'center',
                        minHeight: 44,
                        maxHeight: 44,
                        pt: 0.5,
                        backgroundColor: theme.palette.background.paperLight,
                        position: 'sticky',
                        top: 0,
                        zIndex: 1,
                    }}
                >
                    <IconButton
                        sx={{ position: 'absolute', left: 5, top: 5, marginLeft: 0, marginRight: 2 }}
                        size="small"
                        onClick={() => onClose()}
                    >
                        <CloseIcon sx={{ fontSize: 21 }} />
                    </IconButton>
                    <Typography variant="h6" component="div" sx={{ textAlign: 'center' }}>
                        Daily login report
                    </Typography>
                </Box>

                <Box
                    sx={{
                        pt: 2,
                        overflow: 'auto',
                        backgroundColor: theme.palette.background.default,
                        borderRadius: `${theme.shape.borderRadius}px ${theme.shape.borderRadius}px 0 0`,
                    }}
                >
                    {
                        report &&
                        <ComponentBox
                            title='Report details'
                            icon={<ArticleOutlinedIcon />}
                            isCollapseable
                            sx={{ mt: 0 }}
                        >
                            <TableContainer component={Box} sx={{ borderRadius: 0 }}>
                                <Table
                                    sx={{
                                        '& tbody tr:last-child td, & tbody tr:last-child th': {
                                            borderBottom: 'none',
                                        },
                                    }}
                                >
                                    <TableHead>
                                        <TableRow>
                                            <PaddedTableCell sx={{ width: '200px' }}>Attribute</PaddedTableCell>
                                            <PaddedTableCell>Value</PaddedTableCell>
                                        </TableRow>
                                    </TableHead>
                                    <TableBody>
                                        <TextTableRow key='startTime' keyValue={"Start Time"} value={formatTime(report.startTime)} />
                                        <TextTableRow key='endTime' keyValue={"End Time"} value={isUtcZero ? '' : formatTime(report.endTime)} />
                                        {reportDuration && <TextTableRow key='duration' keyValue={"Run Duration"} value={reportDuration} />}
                                        <TextTableRow key='amountOfAccounts' keyValue={"Amount of Accounts"} value={report.amountOfAccounts} />
                                        <TextTableRow key='amountOfAccountsSucceeded' keyValue={"Logins Succeeded"} value={report.amountOfAccountsSucceeded} />
                                        <TextTableRow key='amountOfAccountsFailed' keyValue={"Logins Failed"} value={report.amountOfAccountsFailed} />
                                        <TextTableRow key='hasFinished' keyValue={"Has Finished"} value={report.hasFinished ? <CheckCircleOutlinedIcon /> : <CancelOutlinedIcon />} />
                                    </TableBody>
                                </Table>
                            </TableContainer>
                        </ComponentBox>
                    }
                    {
                        reportData.failed.length > 0 &&
                        <ComponentBox
                            title='Failed logins'
                            icon={<CancelOutlinedIcon />}
                            isCollapseable
                        >
                            <DailyLoginsLoginData logins={reportData.failed} mascot={'/mascot/Error/error_2_low_res.png'} />
                        </ComponentBox>
                    }
                    {
                        reportData.succeeded.length > 0 &&
                        <ComponentBox
                            title='Successful logins'
                            icon={<CheckCircleOutlinedIcon />}
                            isCollapseable
                        >
                            <DailyLoginsLoginData logins={reportData.succeeded} />
                        </ComponentBox>
                    }
                </Box>
                <Box
                    sx={{
                        flex: '1 0 auto',
                        height: '0px',
                        backgroundColor: theme.palette.background.default,
                    }}
                />
            </Drawer>
        </Box>
    );
}

export default DailyLoginsSlideout;

function DailyLoginsLoginData({ logins, mascot }) {
    const theme = useTheme();
    const navigate = useNavigate();

    return (
        <Box
            sx={{
                position: 'relative',
                width: '100%',
                overflowX: 'hidden',
            }}
        >
            <Typography variant="body2" sx={{ p: 1 }}>
                Click on a row to view the account details
            </Typography>
            {
                mascot &&
                <img
                    src={mascot}
                    alt="Okta Mascot"
                    style={{
                        position: 'absolute',
                        top: -6,
                        right: 6,
                        height: '56px',
                    }}
                />
            }
            <TableContainer component={Box} sx={{ borderRadius: 0, maxWidth: '100%', overflow: 'hidden' }}>
                <Table
                    sx={{
                        '& tbody tr:last-child td, & tbody tr:last-child th': {
                            borderBottom: 'none',
                        },
                        '& tbody tr:hover': {
                            backgroundColor: 'rgba(0, 0, 0, 0.04)',
                        },
                        '& tbody tr': {
                            cursor: 'pointer',
                            borderRadius: theme.shape.borderRadius,
                        },
                    }}
                >
                    <TableHead>
                        <TableRow >
                            <TableCell
                                sx={{
                                    pl: 4,
                                    textOverflow: 'ellipsis',
                                    overflow: 'hidden',
                                    whiteSpace: 'nowrap',
                                    maxWidth: '225px'
                                }}
                            >
                                Email
                            </TableCell>
                            <TableCell sx={{ overflow: 'hidden', textAlign: 'center' }}>Start time</TableCell>
                            <TableCell sx={{ overflow: 'hidden', textAlign: 'center' }}>End time</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {
                            logins.map((login) => {
                                return (
                                    <TableRow
                                        key={login.id}
                                        onClick={() => navigate(`/accounts?selectedAccount=${encodeURIComponent(login.accountEmail)}`)}
                                    >
                                        <TableCell
                                            sx={{
                                                pl: 4,
                                                textOverflow: 'ellipsis',
                                                overflow: 'hidden',
                                                whiteSpace: 'nowrap',
                                                width: '100%',
                                            }}
                                        >
                                            {login.accountEmail}
                                        </TableCell>
                                        <TableCell sx={{ overflow: 'hidden', textAlign: 'center', maxWidth: '100px' }}>{formatTime(login.startTime)}</TableCell>
                                        <TableCell sx={{ overflow: 'hidden', textAlign: 'center', maxWidth: '100px' }}>{formatTime(login.endTime)}</TableCell>
                                    </TableRow>
                                );
                            })
                        }
                    </TableBody>
                </Table>
            </TableContainer>
        </Box>
    );
}