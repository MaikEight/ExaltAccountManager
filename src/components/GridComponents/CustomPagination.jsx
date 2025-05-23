import React, { forwardRef } from 'react';
import { Pagination, TablePagination } from "@mui/material";
import { gridPageCountSelector, gridPageSelector, gridPageSizeSelector, gridRowCountSelector, useGridApiContext, useGridSelector } from "@mui/x-data-grid";

const CustomPagination = forwardRef(function CustomPagination({ rowsPerPageOptions, labelRowsPerPage }, ref) {
    const gridApiContext = useGridApiContext();
    const page = useGridSelector(gridApiContext, gridPageSelector);
    const rowCount = useGridSelector(gridApiContext, gridRowCountSelector);
    const pageCount = useGridSelector(gridApiContext, gridPageCountSelector);
    const pageSize = useGridSelector(gridApiContext, gridPageSizeSelector);

    const handleChangePage = (event, newPage) => {
        gridApiContext.current.setPage(newPage);
    };

    const handleChangePageSize = (event) => {
        gridApiContext.current.setPageSize(parseInt(event.target.value, 10));
        gridApiContext.current.setPage(0);
    };

    return (
        <TablePagination
            ref={ref}
            className="pagination-container"
            style={{ flex: '1 0 auto' }}
            component="div"
            count={rowCount}
            page={page}
            rowsPerPage={pageSize}
            onRowsPerPageChange={handleChangePageSize}
            onPageChange={handleChangePage}
            rowsPerPageOptions={rowsPerPageOptions}
            labelDisplayedRows={() => { return ''; }}
            labelRowsPerPage={labelRowsPerPage}
            ActionsComponent={() => (
                <Pagination
                    className="pagination-pages"
                    style={{ flex: '1 0 auto', paddingLeft: '1rem' }}
                    color="primary"
                    shape="rounded"
                    page={page + 1}
                    count={pageCount}
                    onChange={(event, newPage) => handleChangePage(event, newPage - 1)}
                />
            )}
        />
    );
});

export { CustomPagination };