import { Box, Divider, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography, Chip } from "@mui/material";
import ComponentBox from "./ComponentBox";
import AddCircleOutlineOutlinedIcon from '@mui/icons-material/AddCircleOutlineOutlined';
import PaddedTableCell from './AccountDetails/PaddedTableCell';
import AllInclusiveIcon from '@mui/icons-material/AllInclusive';
import CheckCircleOutlineOutlinedIcon from '@mui/icons-material/CheckCircleOutlineOutlined';
import CancelOutlinedIcon from '@mui/icons-material/CancelOutlined';
import { useEffect, useState } from "react";
import UnfoldMoreIcon from '@mui/icons-material/UnfoldMore';
import FormatListBulletedIcon from '@mui/icons-material/FormatListBulleted';
import StyledButton from "./StyledButton";
import LoginOutlinedIcon from '@mui/icons-material/LoginOutlined';
import AddShoppingCartOutlinedIcon from '@mui/icons-material/AddShoppingCartOutlined';
import { getEamPlusPrices, useUserLogin } from 'eam-commons-js';
import EngineeringOutlinedIcon from '@mui/icons-material/EngineeringOutlined';
import { useColorList } from "../hooks/useColorList";

function EamPlusComparisonTable() {
    const { user } = useUserLogin();

    const [expandedAccounts, setExpandedAccounts] = useState(false);
    const [expandedVP, setExpandedVP] = useState(false);
    const [prices, setPrices] = useState(null);

    const trueIcon = <CheckCircleOutlineOutlinedIcon color="success" />;
    const falseIcon = <CancelOutlinedIcon color="error" />;
    const infinityIcon = <AllInclusiveIcon color="success" />;
    const workInProgressIcon = <EngineeringOutlinedIcon color="primary" />;
    const chipColor = useColorList(0);

    useEffect(() => {
        const fetchPrices = async () => {
            const storedPrices = sessionStorage.getItem('eam_plus_prices');
            if (storedPrices !== null) {
                setPrices(new Map(JSON.parse(storedPrices)));
                return;
            }

            const priceDataStr = await getEamPlusPrices();
            const priceData = JSON.parse(priceDataStr);
            let _prices = ['monthly', 'yearly', 'permanent']
                .map(lookup_key => {
                    const p = priceData.find(p => p.lookup_key === lookup_key);
                    return {
                        key: lookup_key,
                        data: {
                            ...p,
                            displayPrice: p.unit_amount / 100 + '€'
                        }
                    };
                }).filter(p => p.data !== undefined);
            _prices = new Map(_prices.map(p => [p.key, p.data]));
            sessionStorage.setItem('eam_plus_prices', JSON.stringify([..._prices]));
            setPrices(_prices);
        };
        fetchPrices();
    }, []);

    const checkout = (variant) => {
        if (sessionStorage.getItem('flag:debug') === 'true') {
            console.log('checkout', variant, user);
        }
    };

    const pricesTable = () => {
        if (!prices) {
            return null;
        }

        return (
            <TableContainer>
                <Table
                    sx={{
                        '& tbody  td, & tbody th': {
                            borderBottom: 'none',
                        },
                    }}
                >
                    <TableHead>
                        <TableRow>
                            <PaddedTableCell sx={{ textAlign: 'start', borderBottom: 'none', py: 0 }}>
                                <Typography variant="h6" fontWeight={300}>
                                    Plan
                                </Typography>
                            </PaddedTableCell>
                            <PaddedTableCell sx={{ textAlign: 'start', borderBottom: 'none', py: 0 }}>
                                <Typography variant="h6" fontWeight={300}>
                                    Price
                                </Typography>
                            </PaddedTableCell>
                            <PaddedTableCell sx={{ textAlign: 'start', borderBottom: 'none', py: 0 }}>
                            </PaddedTableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        <PriceTableRow
                            plan="EAM Plus Monthly"
                            price={`/ Monthly`}
                            priceData={prices.get('monthly')}
                            onClick={() => checkout('monthly')}
                        />
                        <PriceTableRow
                            plan={
                                <Box
                                    sx={{
                                        display: 'flex',
                                        flexDirection: 'row',
                                        gap: 1,
                                        alignItems: 'center',
                                    }}
                                >
                                    <Typography variant="body1" fontWeight={300}>
                                        EAM Plus Yearly
                                    </Typography>
                                    <Chip
                                        label="Recommended"
                                        size="small"
                                        sx={{
                                            backgroundColor: chipColor.background,
                                            color: chipColor.color,
                                        }}
                                    />
                                </Box>
                            }
                            priceData={prices.get('yearly')}
                            price={` / Year`}
                            onClick={() => checkout('yearly')}
                        />
                        <PriceTableRow
                            plan="EAM Plus Permanent"
                            price={''}
                            priceData={prices.get('permanent')}
                            onClick={() => checkout('permanent')}
                        />
                    </TableBody>
                </Table>
            </TableContainer>
        );
    };

    return (
        <ComponentBox
            title="EAM Feature Plans"
            icon={<FormatListBulletedIcon />}
            innerSx={{
                display: 'flex',
                flexDirection: 'column',
                alignItems: 'center',
            }}
            defaultCollapsed={user?.isPlusUser}
            isCollapseable={user?.isPlusUser}
        >
            {
                !user?.isPlusUser &&
                <Box
                    sx={{
                        width: '100%',
                        mb: 4,
                        p: 1,
                        borderRadius: theme => `${theme.shape.borderRadius}px`,
                        backgroundColor: theme => theme.palette.background.default,
                    }}
                >
                    {pricesTable()}
                </Box>
            }
            <Box
                sx={{
                    width: '600px',
                }}
            >
                <TableContainer>
                    <Table
                        sx={{
                            '& tbody  td, & tbody th': {
                                borderBottom: 'none',
                                padding: 0,
                                paddingTop: 0.25,
                                paddingBottom: 1,
                            },
                        }}
                    >
                        <TableHead>
                            <TableRow>
                                <PaddedTableCell sx={{ width: '200px', textAlign: 'left', borderBottom: 'none', py: 0 }}></PaddedTableCell>
                                <PaddedTableCell sx={{ textAlign: 'center', borderBottom: 'none', py: 0 }}>
                                    <Typography variant="h6" fontWeight={300}>
                                        Free
                                    </Typography>
                                </PaddedTableCell>
                                <PaddedTableCell sx={{ textAlign: 'center', borderBottom: 'none', py: 0 }}>
                                    <Typography variant="h6" fontWeight={300}>
                                        Signed in
                                    </Typography>
                                </PaddedTableCell>
                                <PaddedTableCell
                                    sx={{
                                        textAlign: 'center',
                                        display: 'flex',
                                        flexDirection: 'row',
                                        justifyContent: 'center',
                                        alignItems: 'center',
                                        gap: 1,
                                        borderBottom: 'none',
                                        py: 0
                                    }}
                                >

                                    <Typography variant="h6" fontWeight={300}>
                                        Plus
                                    </Typography>
                                    <AddCircleOutlineOutlinedIcon />
                                </PaddedTableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            <ComparisonTableCategoryRow category="Accounts" />
                            <ComparisonTableRow
                                attribute="Amount of accounts"
                                defaultValue={infinityIcon}
                                signedInValue={infinityIcon}
                                eamPlusValue={infinityIcon}
                            />
                            <ComparisonTableRow
                                attribute="Steam account support"
                                defaultValue={trueIcon}
                                signedInValue={trueIcon}
                                eamPlusValue={trueIcon}
                            />
                            {
                                expandedAccounts ?
                                    <>
                                        <ComparisonTableRow
                                            attribute="Filtering"
                                            defaultValue={trueIcon}
                                            signedInValue={trueIcon}
                                            eamPlusValue={trueIcon}
                                        />
                                        <ComparisonTableRow
                                            attribute="Once Click Data Export"
                                            defaultValue={trueIcon}
                                            signedInValue={trueIcon}
                                            eamPlusValue={trueIcon}
                                        />
                                        <ComparisonTableRow
                                            attribute="Custom view"
                                            defaultValue={trueIcon}
                                            signedInValue={trueIcon}
                                            eamPlusValue={trueIcon}
                                        />
                                    </>
                                    :
                                    <ComparisonTableExpandRow onClick={() => setExpandedAccounts(true)} />
                            }

                            <ComparisonTableCategoryRow category="Vault Peeker" />
                            {
                                expandedVP ?
                                    <>
                                        <ComparisonTableRow
                                            attribute="Extensive filter options"
                                            defaultValue={trueIcon}
                                            signedInValue={trueIcon}
                                            eamPlusValue={trueIcon}
                                        />
                                        <ComparisonTableRow
                                            attribute="Export Totals"
                                            defaultValue={trueIcon}
                                            signedInValue={trueIcon}
                                            eamPlusValue={trueIcon}
                                        />
                                        <ComparisonTableRow
                                            attribute="View Character Stats & Equipment"
                                            defaultValue={trueIcon}
                                            signedInValue={trueIcon}
                                            eamPlusValue={trueIcon}
                                        />
                                    </>
                                    :
                                    <ComparisonTableExpandRow onClick={() => setExpandedVP(true)} />
                            }
                            <ComparisonTableCategoryRow category="Daily Logins" />
                            <ComparisonTableRow
                                attribute="Amount of accounts"
                                defaultValue={infinityIcon}
                                signedInValue={infinityIcon}
                                eamPlusValue={infinityIcon}
                            />
                            <ComparisonTableRow
                                attribute="Faster & ressource friendly login"
                                defaultValue={falseIcon}
                                signedInValue={falseIcon}
                                eamPlusValue={trueIcon}
                            />
                            <ComparisonTableRow
                                attribute="Custom timing settings"
                                defaultValue={falseIcon}
                                signedInValue={falseIcon}
                                eamPlusValue={workInProgressIcon}
                            />
                            <ComparisonTableCategoryRow category="Miscellaneous" />
                            <ComparisonTableRow
                                attribute="Settings Sync"
                                defaultValue={falseIcon}
                                signedInValue={workInProgressIcon}
                                eamPlusValue={workInProgressIcon}
                            />
                            <ComparisonTableRow
                                attribute="Discord Role"
                                defaultValue={falseIcon}
                                signedInValue={trueIcon}
                                eamPlusValue={trueIcon}
                            />
                            <ComparisonTableRow
                                attribute="Support the project"
                                defaultValue={falseIcon}
                                signedInValue={falseIcon}
                                eamPlusValue={trueIcon}
                            />
                        </TableBody>
                    </Table>
                </TableContainer>
            </Box>

            <Box
                sx={{
                    display: 'flex',
                    flexDirection: 'column',
                    alignItems: 'center',
                }}
            >
                <Typography variant="caption" sx={{ mt: 1 }}>
                    * All features are subject to change and may be removed or added at any time.
                </Typography>
            </Box>
        </ComponentBox>
    );
}

function PriceTableRow({ plan, price, priceData, onClick, sx }) {
    const { isAuthenticated, user, login } = useUserLogin();
    price = priceData ? `${priceData.displayPrice} ${price}` : price;

    const getPaymentMode = (type) => {
        switch (type) {
            case 'one_time':
                return 'payment';
            case 'recurring':
                return 'subscription';
            default:
                return 'Unknown';
        }
    }

    return (
        <TableRow
            sx={{
                ...sx
            }}
            onClick={onClick}
            hover
        >
            <PaddedTableCell
                sx={{
                    borderRadius: theme => `${theme.shape.borderRadius}px 0 0 ${theme.shape.borderRadius}px`
                }}
            >
                <Box
                    sx={{
                        height: '100%',
                        display: 'flex',
                        flexDirection: 'row',
                        alignItems: 'center',
                    }}
                >
                    {
                        typeof plan === 'string' ?
                            <Typography variant="body1" fontWeight={300}>
                                {plan}
                            </Typography>
                            :
                            plan
                    }
                </Box>
            </PaddedTableCell>
            <PaddedTableCell
                sx={{
                    py: 0,
                }}
            >
                <Typography variant="body1" fontWeight={300}>
                    {price}
                </Typography>
            </PaddedTableCell>
            <PaddedTableCell
                sx={{
                    py: 0,
                    minWidth: '0px',
                    width: 'fit-content',
                    borderRadius: theme => `0 ${theme.shape.borderRadius}px ${theme.shape.borderRadius}px 0`
                }}
            >
                {
                    (isAuthenticated && user) ?
                        <form action="https://payments.exaltaccountmanager.com/eam-plus-create-checkout-session.php" target="_blank" method="POST">
                            <input type="hidden" name="lookup_key" value={priceData.id} />
                            <input type="hidden" name="mode" value={getPaymentMode(priceData.type)} />
                            <input type="hidden" name="client_reference_id" value={user.user_id ?? user.sub ?? null} />
                            <StyledButton
                                startIcon={<AddShoppingCartOutlinedIcon />}
                                type="submit"
                                id="checkout-button"
                                sx={{
                                    width: "fit-content",
                                }}
                            >
                                Choose Plan
                            </StyledButton>
                        </form>
                        :
                        <StyledButton
                            sx={{
                                width: "fit-content"
                            }}
                            startIcon={<LoginOutlinedIcon />}
                            onClick={login}
                        >
                            Log In
                        </StyledButton>
                }
            </PaddedTableCell>
        </TableRow>
    );
}

function ComparisonTableRow({ attribute, defaultValue, signedInValue, eamPlusValue }) {
    return (
        <TableRow>
            <TableCell><Box sx={{ pl: 1.5 }}>{attribute}</Box></TableCell>
            <TableCell sx={{ textAlign: 'center' }}>{defaultValue}</TableCell>
            <TableCell sx={{ textAlign: 'center' }}>{signedInValue}</TableCell>
            <TableCell sx={{ textAlign: 'center' }}>{eamPlusValue}</TableCell>
        </TableRow>
    );
}

function ComparisonTableCategoryRow({ category, hideDivider }) {
    return (
        <TableRow>
            <TableCell>
                <Typography variant="h6" fontWeight={300}>
                    {category}
                </Typography>
            </TableCell>
            <TableCell colSpan={3}>
                {!hideDivider && <Divider />}
            </TableCell>
        </TableRow>
    );
}
function ComparisonTableExpandRow({ onClick }) {
    return (
        <TableRow sx={{ p: 0 }}>
            <TableCell sx={{ p: 0 }} />
            <TableCell sx={{ p: 0 }} />
            <TableCell
                sx={{ p: 0 }}
            >
                <Box
                    sx={{
                        display: 'flex',
                        flexDirection: 'row',
                        justifyContent: 'center',
                        alignItems: 'center',
                    }}
                >
                    <Chip
                        onClick={onClick}
                        label="Show more"
                        size="small"
                        icon={<UnfoldMoreIcon />}
                    />
                </Box>
            </TableCell>
            <TableCell sx={{ p: 0 }} />
        </TableRow>
    );
}

export default EamPlusComparisonTable;