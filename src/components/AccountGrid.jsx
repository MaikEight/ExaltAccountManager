import { useTheme } from "@emotion/react";
import { Button, Checkbox, Chip, Paper } from "@mui/material";
import { DataGrid, GridToolbarColumnsButton, GridToolbarContainer } from '@mui/x-data-grid';
import { useState } from "react";
import styled from "styled-components";
import useStringToColor from "../hooks/useStringToColor";
import StyledButton from "./StyledButton";
import { CustomPagination } from "./GridComponents/CustomPagination";
import ServerChip from "./GridComponents/ServerChip";
import DailyLoginCheckbox from "./GridComponents/DailyLoginCheckbox";

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
    {
      id: 4,
      name: 'Jane Smith',
      email: 'j.smith@email.de',
      lastLogin: '12-12-2023 10:11',
      performDailyLogin: false,
      serverName: 'EUSouthWest3',
      lastRefresh: '12-12-2023 10:10',
    },
    {
      id: 5,
      name: 'Lorem Ipsum et dolor',
      email: 'asdasd@asda.et',
      lastLogin: '12-12-2023 10:11',
      performDailyLogin: false,
      serverName: 'USEast',
    },
    {
      id: 6,
      name: 'Lorem Ipsum',
      email: 'lorem@Ipsum.lt',
      lastLogin: '12-12-2023 10:11',
      performDailyLogin: false,
      serverName: 'Asia',
    }
  ]);
  const [selectedAccount, setSelectedAccount] = useState(null);
  const [paginationModel, setPaginationModel] = useState({ page: 0, pageSize: 10 });

  const theme = useTheme();

  const columns = [
    { field: 'name', headerName: 'Name', minWidth: 65, flex: 0.2 },
    { field: 'email', headerName: 'Email', minWidth: 65, flex: 0.3 },
    { field: 'lastLogin', headerName: 'Last Login', minWidth: 140, flex: 0.15 },
    { field: 'serverName', headerName: 'Server', width: 125, renderCell: (params) => <ServerChip params={params} /> },
    { field: 'lastRefresh', headerName: 'Last refresh', minWidth: 140, flex: 0.15 },
    { field: 'performDailyLogin', headerName: 'Daily Login', width: 95, renderCell: (params) => <DailyLoginCheckbox params={params} onChange={(event) => { setAccounts((prev) => prev.map((account) => account.id === params.id ? { ...account, performDailyLogin: event.target.checked } : account));}} /> },
    { field: 'id', headerName: '', width: 95, renderCell: (params) => PlayButton(params) },
  ];

  const PlayButton = (params) => (
      <StyledButton
        sx={{ margin: 'auto' }}
        onClick={() => {
          console.log("Play button clicked: ", params);          
        }}
      >
        Play
      </StyledButton>
  );

  return (
    <Paper sx={{ height: 'calc(90vh - 70px)', width: '100%', borderRadius: 1.5, background: theme.palette.background.paper, }}>
      <StyledDataGrid
        sx={{ border: 0, '&, [class^=MuiDataGrid]': { border: 'none' } }}
        rows={accounts}
        getRowId={(row) => row.id}
        columns={columns}        
        pageSizeOptions={[1]}
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
        paginationModel={paginationModel}
        onPaginationModelChange={setPaginationModel}
        checkboxSelection={false}
        hideFooterSelectedRowCount
        slots={{
          pagination: CustomPagination,
        }}
      />
    </Paper>
  );
}

export default AccountGrid;