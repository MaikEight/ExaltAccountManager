import { Box, List, ListItem, ListItemText, ListSubheader, Paper, Table, TableBody, TableCell, TableContainer, TableRow, Typography } from '@mui/material';
import { useTheme } from "@emotion/react";
import { useGroups, GroupUI, useUserLogin } from 'eam-commons-js';
import { useEffect, useState } from 'react';
import { useMemo } from 'react';
import { useTransition, animated } from '@react-spring/web';
import { invoke } from '@tauri-apps/api/core';
import './DashboardPage.css';
import useWorkers from './../hooks/useWorkers';
import { formatTime } from 'eam-commons-js';

const getDurationString = (start, end) => {
    if (!start || !end) return 'N/A';
    if (!(start instanceof Date) || !(end instanceof Date)) return 'N/A';
    const diff = end - start;
    if (diff < 0) return 'N/A';
    return new Date(diff).toISOString().substr(11, 8);
}

const getEndTimeEstimate = (accountsLeft, secondsPerAccount) => {
    if (!accountsLeft || !secondsPerAccount) return 'N/A';
    const seconds = accountsLeft * secondsPerAccount;
    return formatTime(new Date(Date.now() + (seconds * 1000)));
}

function DashboardPage() {
    const { accountsToPerformDailyLoginFor, accountsDone, report } = useWorkers();
    const { groups } = useGroups();
    const { idToken } = useUserLogin();
    const theme = useTheme();

    // Keep one array, each item with a status
    const [accounts, setAccounts] = useState([]);
    const [usesPlusMode, setUsesPlusMode] = useState(false);
    const [durationString, setDurationString] = useState('N/A');

    const getGroup = (acc) => {
        if (!groups) return null;
        if (!acc) return null;
        if (!acc.group) return null;

        return groups.find(g => g.name === acc.group);
    }
    useEffect(() => {
        if (!idToken) {
            setUsesPlusMode(false);
            return;
        }
        const result = invoke('is_plus_user', { idToken: idToken }).catch(console.error);
        setUsesPlusMode(Boolean(result));
    }, [idToken]);

    useEffect(() => {
        const todos = accountsToPerformDailyLoginFor.map(a => ({ ...a, tempStatus: 'todo' }));
        const dones = accountsDone.map(a => ({ ...a, tempStatus: 'done' }));
        const accs = [...todos, ...dones];
        // Remove duplicates, keep the last one
        const unique = accs.filter((v, i, a) => a.findIndex(t => (t.id === v.id)) === i);
        setAccounts((prev) => unique);
    }, [accountsToPerformDailyLoginFor, accountsDone]);

    // Sort by status then ID
    const sorted = useMemo(() => {
        return [...accounts].sort((a, b) => {
            if (a.tempStatus === b.tempStatus) {
                return a.id - b.id;
            }
            return a.tempStatus === 'todo' ? -1 : 1;
        });
    }, [accounts]);

    useEffect(() => {
        const interValId = setInterval(() => {
            setDurationString(getDurationString(new Date(report.startTime), new Date(Date.now())));
        }, 1000);

        return () => clearInterval(interValId);
    }, [report]);

    // Animate layout changes
    const transitions = useTransition(sorted, {
        keys: item => item.id,
        from: { transform: 'translateY(0)' },
        enter: { transform: 'translateY(0)' },
        leave: { transform: 'translateY(0)' },
        layout: true
    });

    return (
        <Box
            sx={{
                display: 'flex',
                flexDirection: 'column',
                width: '100%',
                pl: 2,
                gap: 1,
            }}
        >
            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'row',
                    gap: 2,
                }}
            >
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        p: 1,
                        borderRadius: `${theme.shape.borderRadius}px`,
                        backgroundColor: theme.palette.background.paper,
                    }}
                >
                    <Typography variant="h6" sx={{ px: 0.5 }}>Status Report</Typography>
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            pt: 0.5,
                            pb: 0.25,
                            px: 0.25,
                        }}
                    >
                        <TableContainer component={Box} sx={{ borderRadius: 0 }}>
                            <Table
                                sx={{
                                    '& tbody  td, & tbody th': {
                                        borderBottom: 'none',
                                    },
                                }}
                            >
                                <TableBody>
                                    <TableRow>
                                        <TableCell sx={{ p: 0 }}>🔵 Progress</TableCell>
                                        <TableCell sx={{ p: 0, pl: 1 }}>{report?.amountOfAccountsProcessed} / {report?.amountOfAccounts}</TableCell>
                                    </TableRow>
                                    <TableRow>
                                        <TableCell sx={{ p: 0, pt: 0.5 }}>🟢 Success</TableCell>
                                        <TableCell sx={{ p: 0, pt: 0.5, pl: 1 }}>{report?.amountOfAccountsSucceeded}</TableCell>
                                    </TableRow>
                                    <TableRow>
                                        <TableCell sx={{ p: 0, pt: 0.5 }}>🔴 Failed</TableCell>
                                        <TableCell sx={{ p: 0, pt: 0.5, pl: 1 }}>{report?.amountOfAccountsFailed}</TableCell>
                                    </TableRow>
                                </TableBody>
                            </Table>
                        </TableContainer>
                    </Box>
                </Box>
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        p: 1,
                        borderRadius: `${theme.shape.borderRadius}px`,
                        backgroundColor: theme.palette.background.paper,
                    }}
                >
                    <Typography variant="h6" sx={{ px: 0.5 }}>Time</Typography>
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            pt: 0.5,
                            pb: 0.25,
                            px: 0.25,
                        }}
                    >
                        <TableContainer component={Box} sx={{ borderRadius: 0 }}>
                            <Table
                                sx={{
                                    '& tbody  td, & tbody th': {
                                        borderBottom: 'none',
                                    },
                                }}
                            >
                                <TableBody>
                                    <TableRow>
                                        <TableCell sx={{ p: 0 }}>🕛 Start time</TableCell>
                                        <TableCell sx={{ p: 0, pl: 1 }}>{formatTime(report?.startTime)}</TableCell>
                                    </TableRow>
                                    <TableRow>
                                        <TableCell sx={{ p: 0, pt: 0.5 }}>⏱️ Duration</TableCell>
                                        <TableCell sx={{ p: 0, pt: 0.5, pl: 1 }}>{durationString}</TableCell>
                                    </TableRow>
                                    <TableRow>
                                        <TableCell sx={{ p: 0, pt: 0.5 }}>🕕 End Time</TableCell>
                                        <TableCell sx={{ p: 0, pt: 0.5, pl: 1 }}>~{report ? getEndTimeEstimate(report.amountOfAccounts - report.amountOfAccountsProcessed, usesPlusMode ? 60 : 90) : 'N/A'}</TableCell>
                                    </TableRow>
                                </TableBody>
                            </Table>
                        </TableContainer>
                    </Box>
                </Box>
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'column',
                        p: 1,
                        borderRadius: `${theme.shape.borderRadius}px`,
                        backgroundColor: theme.palette.background.paper,
                    }}
                >
                    <Typography variant="h6" sx={{ px: 0.5 }}>Daily Login Mode</Typography>
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            pt: 0.5,
                            pb: 0.25,
                            px: 0.25,
                        }}
                    >
                        {
                            usesPlusMode ? (
                                <>
                                    <Typography variant="body1">🎉 Plus Mode</Typography>
                                    <Typography variant="body1">🏁 Fast & steady</Typography>
                                </>
                            ) : (
                                <>
                                    <Typography variant="body1">🟢 Normal Mode</Typography>
                                </>
                            )
                        }
                    </Box>
                </Box>
            </Box>
            <List
                sx={{
                    width: '100%',
                    overflow: 'auto',
                    pr: 1,
                }}
                disablePadding
            >
                {transitions((style, item, _, i) => {
                    let heading = null;
                    if (i === 0 || sorted[i - 1]?.tempStatus !== item?.tempStatus) {
                        heading = (
                            <ListSubheader sx={{ backgroundColor: theme.palette.background.default }}>
                                <Typography variant="h6">
                                    {item.tempStatus === 'todo' ? 'In Progress' : 'Done'}
                                </Typography>
                            </ListSubheader>
                        );
                    }
                    return (
                        <>
                            {heading}
                            <animated.div id={item.id} style={style}>
                                <AccountListItem account={item} group={getGroup(item)} />
                            </animated.div>
                        </>
                    );
                })}
            </List>
        </Box>
    );
}

export default DashboardPage;

function AccountListItem({ account, group }) {
    const theme = useTheme();

    return (
        <ListItem disablePadding>
            <ListItemText>
                <Box
                    sx={{
                        display: 'flex',
                        alignItems: 'center',
                        justifyContent: 'start',
                        p: 1,
                        borderRadius: `${theme.shape.borderRadius}px`,
                        backgroundColor: theme.palette.background.paper,
                        gap: 2,
                    }}
                >
                    <Box
                        sx={{
                            display: 'flex',
                            alignItems: 'center',
                            justifyContent: 'center',
                            width: 30,
                        }}
                    >
                        <GroupUI group={group} />
                    </Box>
                    <Box
                        sx={{
                            display: 'flex',
                            flexDirection: 'column',
                            alignItems: 'start',
                            justifyContent: 'start',
                        }}
                    >
                        <Typography variant='body1'>{account.name}</Typography>
                        <Typography variant='subtitle2'>{account.email}</Typography>
                    </Box>
                </Box>
            </ListItemText>
        </ListItem>
    );
}