import { useTheme } from "@emotion/react";
import { Paper } from "@mui/material";
import { DataGrid } from '@mui/x-data-grid';
import { useState } from "react";
import styled from "styled-components";

const StyledDataGrid = styled(DataGrid)`
  &.MuiDataGrid-root .MuiDataGrid-colHeader {
    background-color: '#ffffff';
    color: '#ffffff';
  }
  &.MuiDataGrid-root .MuiDataGrid-columnHeader:focus,
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
    },
    {
      id: 2,
      name: 'Jane Doe',
      email: 'jane.doe@email.com',
      lastLogin: '10-12-2023 18:20',
      performDailyLogin: true,
    },
    {
      id: 3,
      name: 'John Smith',
      email: 'john@smith.de',
      lastLogin: '12-12-2023 10:11',
      performDailyLogin: false,
    },
  ]);

  const theme = useTheme();

  const columns = [
    { field: 'name', headerName: 'Name', minWidth: 65, flex: 0.15 },
    { field: 'email', headerName: 'Email', minWidth: 65, flex: 0.15 },
    { field: 'lastLogin', headerName: 'Last Login', minWidth: 65, flex: 0.15 },
    { field: 'performDailyLogin', headerName: 'Daily Login', minWidth: 65, flex: 0.15 },
  ];

  return (
    <Paper sx={{ height: 'calc(90vh - 70px)', width: '100%', borderRadius: 1.5, background: theme.palette.background.paper, }}>
      <StyledDataGrid
        sx={{ border: 0}}
        rows={accounts}
        columns={columns}
        pageSize={100}
        rowsPerPageOptions={[100]}
        getRowHeight={() => "52"}
        getEstimatedRowHeight={() => 52}
        rowCount={accounts.length}
        density="compact"
      />
    </Paper>
  );
}

export default AccountGrid;