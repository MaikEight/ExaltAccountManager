import { Box, Pagination, Paper, TablePagination } from "@mui/material";
import useVaultPeeker from "../../hooks/useVaultPeeker";

const rowsPerPageOptions = [10, 25, 50, 100];

function AccountsPagination() {
    const { page, setPage,
        rowsPerPage, setRowsPerPage, accountsData } = useVaultPeeker();

    const rowCount = accountsData ? accountsData.length : 0;

    // Do not show pagination if there is only one page, except when we are not on the first rowsPerPageOption 
    if (Math.ceil(rowCount / rowsPerPage) - 1 === 0 && rowsPerPage === rowsPerPageOptions[0]) {
        return null;
    }

    return (
        <Box
            sx={{
                position: 'sticky',
                bottom: '0px',
                backgroundColor: theme => theme.palette.background.default,
                zIndex: 10,
                pt: 2,
                mb: 2,
            }}
        >
            <Paper
                sx={{
                    display: 'flex',
                    flexDirection: 'row',
                    justifyContent: 'center',
                    alignItems: 'center',
                    background: theme => theme.palette.background.paper,
                    pr: 1,
                    pl: 1,
                    mx: 2,
                }}
            >
                <TablePagination className="pagination-container"
                    style={{
                        display: 'flex',
                        flex: '1 0 auto',
                        justifyContent: 'end',
                        alignItems: 'center',
                    }}
                    component="div"
                    count={rowCount}
                    page={Math.min(Math.max(page, 0), Math.max(0, Math.ceil(rowCount / rowsPerPage) - 1))}
                    rowsPerPage={rowsPerPage}
                    onRowsPerPageChange={(event) => {
                        const newRowsPerPage = parseInt(event.target.value, 10);
                        const newPage = Math.floor((rowsPerPage * page) / newRowsPerPage);
                        setRowsPerPage(newRowsPerPage);
                        setPage(newPage);
                    }}
                    onPageChange={(event, newPage) => setPage(newPage)}
                    rowsPerPageOptions={rowsPerPageOptions}
                    labelDisplayedRows={() => { return ''; }}
                    labelRowsPerPage={"Accounts per page:"}
                    ActionsComponent={() => (
                        <Pagination className="pagination-pages"
                            style={{ flex: '1 0 auto', }}
                            color="primary"
                            shape="rounded"
                            page={page + 1}
                            count={Math.ceil(rowCount / rowsPerPage)}
                            onChange={(event, page) => setPage(page - 1)}
                        />
                    )}
                />
            </Paper>
        </Box>
    );
}

export default AccountsPagination;