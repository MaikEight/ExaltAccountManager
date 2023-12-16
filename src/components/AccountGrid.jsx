import { useTheme } from "@emotion/react";
import { Box, Button, Checkbox, Chip, Drawer, IconButton, Modal, Paper, Slide, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, Typography, tableCellClasses } from "@mui/material";
import { DataGrid, GridToolbarColumnsButton, GridToolbarContainer } from '@mui/x-data-grid';
import { useState } from "react";
import styled from "styled-components";
import useStringToColor from "../hooks/useStringToColor";
import StyledButton from "./StyledButton";
import { CustomPagination } from "./GridComponents/CustomPagination";
import ServerChip from "./GridComponents/ServerChip";
import DailyLoginCheckbox from "./GridComponents/DailyLoginCheckbox";
import CloseIcon from '@mui/icons-material/Close';

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
      lastLogin: '14-12-2023 10:11',
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
    { field: 'performDailyLogin', headerName: 'Daily Login', width: 95, renderCell: (params) => <DailyLoginCheckbox params={params} onChange={(event) => { setAccounts((prev) => prev.map((account) => account.id === params.id ? { ...account, performDailyLogin: event.target.checked } : account)); }} /> },
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

  const handleCellClick = (params, event) => {
    if( typeof(event.target.type) !== 'undefined') {
          event.stopPropagation();
    }
  };

  return (
    <>
      <Paper sx={{ height: 'calc(100vh - 70px)', width: '100%', borderRadius: 1.5, background: theme.palette.background.paper, }}>
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
      {/* Account details drawer */}
      <Drawer
        sx={{
          width: 0,
          flexShrink: 0,
          '& .MuiDrawer-paper': {
            width: 400,
            boxSizing: 'border-box',
            backgroundColor: theme.palette.background.paper,
            border: 'none',
            borderRadius: '6px 0px 0px 6px',
            boxShadow: ' 0px 0px 20px 10px rgba(0,0,0,0.2)',
            padding: 2,
          },
        }}
        variant="persistent"
        anchor="right"
        open={selectedAccount !== null}
      >
        {/* 
          1. close button - Account details
          2. table with account details
          3. buttons: play, edit, delete          
        */
        }
        {/* 1. */}
        <Box sx={{ display: 'flex', flexDirection: 'row', justifyContent: 'center' }}>
          <IconButton
            sx={{ position: 'absolute', left: 16, marginLeft: 0, marginRight: 2 }}
            size="small"
            onClick={() => setSelectedAccount(null)}
          >
            <CloseIcon />
          </IconButton>
          <Typography variant="h6" component="div" sx={{ textAlign: 'center' }}>
            Account details
          </Typography>
        </Box>
        {/* 2. */}
        <Box
          sx={{
            display: 'flex',
            flexDirection: 'column',
            justifyContent: 'center',
            alignItems: 'center',
            marginTop: 2,
            marginLeft: -2,
            marginRight: -2,
          }}
        >
          <TableContainer component={Paper} sx={{borderRadius: 0}}>
            <Table
              sx={{
                [`& .${tableCellClasses.root}`]: {
                  borderBottom: "none"
                },
              }}
              size="small"
            >
              <TableHead>
                <TableRow>
                  <PaddedTableCell>Attribute</PaddedTableCell>
                  <PaddedTableCell>Value</PaddedTableCell>
                </TableRow>
              </TableHead>
              <TableBody>
                {selectedAccount && Object.keys(selectedAccount).map((key) => (
                  <TableRow key={`${key}`}>
                    <PaddedTableCell>{`${key}`}</PaddedTableCell>
                    <PaddedTableCell>{selectedAccount[key].toString()}</PaddedTableCell>
                  </TableRow>
                ))}
              </TableBody>
            </Table>
          </TableContainer>
        </Box>

      </Drawer >
    </>
  );
}

export default AccountGrid;

function PaddedTableCell({ children, ...props }) {
  return (
    <TableCell {...props} sx={{ pl: 4, pr: 4, pt: 0.5 }}>
      {children}
    </TableCell>
  );
}