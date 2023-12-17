import { useTheme } from "@emotion/react";
import { Paper } from "@mui/material";
import { DataGrid, GridToolbarColumnsButton, GridToolbarContainer } from '@mui/x-data-grid';
import { useEffect, useState } from "react";
import styled from "styled-components";
import { CustomPagination } from "./GridComponents/CustomPagination";
import ServerChip from "./GridComponents/ServerChip";
import DailyLoginCheckbox from "./GridComponents/DailyLoginCheckbox";
import { formatTime } from "../utils/timeUtils";

const StyledDataGrid = styled(DataGrid)`
  &.MuiDataGrid-root .MuiDataGrid-columnHeader:focus,
  &.MuiDataGrid-root .MuiDataGrid-cell:focus-within {
    outline: none;
  },
  &.MuiDataGrid-root .MuiDataGrid-cell:focus {
    outline: none;
  }
`;

function AccountGrid({ acc, selected, setSelected }) {

  const [accounts, setAccounts] = useState(acc);

  useEffect(() => {
    setAccounts(acc);
  }, [acc]);

  useEffect(() => {
    if (selected === selectedAccount) return;

    setSelectedAccount(selected);
  }, [selected]);

  const [selectedAccount, setSelectedAccount] = useState(null);
  const [paginationModel, setPaginationModel] = useState({ page: 0, pageSize: 100 });

  const theme = useTheme();

  const columns = [
    { field: 'name', headerName: 'Accountname', minWidth: 65, flex: 0.2 },
    { field: 'email', headerName: 'Email', minWidth: 65, flex: 0.3 },
    { field: 'lastLogin', headerName: 'Last Login', minWidth: 100, flex: 0.15, type: 'dateTime', valueFormatter: (params) => formatTime(params.value) },
    { field: 'serverName', headerName: 'Server', width: 125, renderCell: (params) => <ServerChip params={params} /> },
    { field: 'lastRefresh', headerName: 'Last refresh', minWidth: 140, flex: 0.15 },
    { field: 'performDailyLogin', headerName: 'Daily Login', width: 95, renderCell: (params) => <DailyLoginCheckbox params={params} onChange={(event) => { setAccounts((prev) => prev.map((account) => account.id === params.id ? { ...account, performDailyLogin: event.target.checked } : account)); }} /> },
  ];

  const handleCellClick = (params, event) => {
    if (typeof (event.target.type) !== 'undefined') {
      event.stopPropagation();
    }
  };

  useEffect(() => {
    setSelected(selectedAccount);
  }, [selectedAccount]);

  return (
    <>
      <Paper sx={{ height: 'calc(100vh - 70px)', width: '100%', borderRadius: 1.5, background: theme.palette.background.paper, }}>
        <StyledDataGrid
          sx={{
            border: 0,
            '&, [class^=MuiDataGrid]': { border: 'none' },
            '& .MuiDataGrid-columnHeaders': {
              backgroundColor: theme.palette.background.paperLight,
            },
            '& .MuiDataGrid-virtualScroller::-webkit-scrollbar': {
              backgroundColor: theme.palette.background.paper,
            },
            '& .MuiDataGrid-virtualScroller::-webkit-scrollbar-thumb': {
              backgroundColor: theme.palette.background.default,
              border: `4px solid ${theme.palette.background.paper}`,
              borderRadius: 1.5
            },
          }}
          rows={accounts}
          getRowId={(row) => row.id}
          columns={columns}
          pageSizeOptions={[10, 25, 50, 100]}
          getRowHeight={() => "auto"}
          rowSelection
          getEstimatedRowHeight={() => 41}
          rowCount={accounts.length}
          onCellClick={handleCellClick}
          onRowSelectionModelChange={(ids) => {
            const selectedId = ids[0];
            const selected = accounts.find((account) => account.id === selectedId);
            if (selected && selected !== selectedAccount) {
              setSelectedAccount(selected);
              return;
            }
            setSelectedAccount(null);
          }}
          rowSelectionModel={selectedAccount ? [selectedAccount.id] : []}
          paginationModel={paginationModel}
          onPaginationModelChange={setPaginationModel}
          checkboxSelection={false}
          hideFooterSelectedRowCount
          slots={{
            pagination: CustomPagination,
          }}
        />
      </Paper>
    </>
  );
}

export default AccountGrid;