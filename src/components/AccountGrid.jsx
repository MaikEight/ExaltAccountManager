import { useTheme } from "@emotion/react";
import { Button, Checkbox, Chip, Paper } from "@mui/material";
import { DataGrid, GridToolbarColumnsButton, GridToolbarContainer } from '@mui/x-data-grid';
import { useState } from "react";
import styled from "styled-components";
import useStringToColor from "../hooks/useStringToColor";
import StyledButton from "./StyledButton";

const StyledDataGrid = styled(DataGrid)`
  &.MuiDataGrid-root .MuiDataGrid-colHeader {
    background-color: '#ffffff';
    color: '#ffffff';
  }
  &.MuiDataGrid-root .MuiDataGrid-columnHeader:focus,
  &.MuiDataGrid-root .MuiDataGrid-cell:focus-within {
    outline: none;
  },
  &.MuiDataGrid-root .MuiDataGrid-cell:focus {
    outline: none;
  }
`;

function AccountGrid() {
  const [accounts, setAccounts] = useState([
    {
      id: 1,
      name: 'John Doe',
      email: 'john.doe@email.com',
      lastLogin: '13-12-2023 12:10',
      performDailyLogin: true,
      serverName: 'EUWest',
      lastRefresh: '13-12-2023 12:10',
    },
    {
      id: 2,
      name: 'Jane Doe',
      email: 'jane.doe@email.com',
      lastLogin: '10-12-2023 18:20',
      performDailyLogin: true,
      serverName: 'USWest2',
      lastRefresh: '13-12-2023 09:10',
    },
    {
      id: 3,
      name: 'John Smith',
      email: 'john@smith.de',
      lastLogin: '12-12-2023 10:11',
      performDailyLogin: false,
      serverName: 'EUSouthWest',
      lastRefresh: '12-12-2023 10:10',
    },
  ]);
  const [selectedAccount, setSelectedAccount] = useState(null);

  const theme = useTheme();

  const columns = [
    { field: 'name', headerName: 'Name', minWidth: 65, flex: 0.2 },
    { field: 'email', headerName: 'Email', minWidth: 65, flex: 0.3 },
    { field: 'lastLogin', headerName: 'Last Login', minWidth: 140, flex: 0.15 },
    { field: 'serverName', headerName: 'Server', width: 120, renderCell: (params) => ServerChip(params) },
    { field: 'lastRefresh', headerName: 'Last refresh', minWidth: 140, flex: 0.15 },
    { field: 'performDailyLogin', headerName: 'Daily Login', width: 95, renderCell: (params) => DailyLoginCheckbox(params) },
    { field: 'id', headerName: '', width: 95, renderCell: (params) => PlayButton(params) },
  ];

  const PlayButton = (params) => (
      <StyledButton
        sx={{ margin: 'auto' }}
        onClick={() => {
          console.log("Play button clicked: ", params);
          //setAccounts((prev) => prev.map((account) => account.id === params.id ? { ...account, performDailyLogin: event.target.checked } : account));
        }}
      >
        Play
      </StyledButton>
  );

  const ServerChip = (params) => (
    <Chip
      sx={{
        margin: 'auto',
        ...useStringToColor(params.value),
      }}
      label={params.value}
      size="small"
    />);

  const DailyLoginCheckbox = (params) => (
    <Checkbox
      sx={{ margin: 'auto' }}
      checked={params.value}
      onChange={(event) => {
        setAccounts((prev) => prev.map((account) => account.id === params.id ? { ...account, performDailyLogin: event.target.checked } : account));
      }}
    />);

  return (
    <Paper sx={{ height: 'calc(90vh - 70px)', width: '100%', borderRadius: 1.5, background: theme.palette.background.paper, }}>
      <StyledDataGrid
        sx={{ border: 0, '&, [class^=MuiDataGrid]': { border: 'none' } }}
        rows={accounts}
        getRowId={(row) => row.id}
        columns={columns}
        pageSize={100}
        rowsPerPageOptions={[100]}
        getRowHeight={() => "auto"}
        rowSelection
        getEstimatedRowHeight={() => 41}
        rowCount={accounts.length}
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
        checkboxSelection={false}
        hideFooterSelectedRowCount
      />
    </Paper>
  );
}

export default AccountGrid;