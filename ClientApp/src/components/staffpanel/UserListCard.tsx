import React, { useEffect, useState } from "react";
import Stack from "@mui/material/Stack";
import CircularProgress from "@mui/material/CircularProgress";
import { SimpleUser } from "../../util/API/APITypes";
import APIClient from "../../util/API/APIClient";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import TableContainer from "@mui/material/TableContainer";
import TableCell from "@mui/material/TableCell";
import TableHead from "@mui/material/TableHead";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableRow from "@mui/material/TableRow";
import AccountBoxIcon from "@mui/icons-material/AccountBox";
import IconButton from "@mui/material/IconButton";
import { Link } from "react-router-dom";
import TextField from "@mui/material/TextField";
import InputAdornment from "@mui/material/InputAdornment";
import SearchIcon from "@mui/icons-material/Search";
import RefreshIcon from "@mui/icons-material/Refresh";
import Tooltip from "@mui/material/Tooltip";
import Typography from "@mui/material/Typography";

const UserListCard = () => {
  const [userList, setUserList] = useState<SimpleUser[]>([]);
  const [isLoaded, setIsLoaded] = useState(false);
  const [searchString, setSearchString] = useState("");

  const refreshUserList = () => {
    setIsLoaded(false);
    APIClient.profiles.all().then((users) => {
      setUserList(users);
      setIsLoaded(true);
    });
  };

  useEffect(() => {
    refreshUserList();
  }, []);

  return (
    <Card className="max-w-screen-md mb-5 mx-auto">
      <CardContent>
        <Typography variant="h5" gutterBottom sx={{ mb: 3 }}>
          User List
        </Typography>

        <Stack direction="row">
          <TextField
            id="user-search-input"
            label="Search"
            InputProps={{
              startAdornment: (
                <InputAdornment position="start">
                  <SearchIcon />
                </InputAdornment>
              ),
            }}
            placeholder="Keyword"
            value={searchString}
            onChange={(e) => setSearchString(e.target.value)}
          />
          <Stack justifyContent="center" sx={{ flexGrow: 0, marginLeft: "auto" }}>
            <Tooltip title="Refresh list">
              <IconButton onClick={refreshUserList}>
                <RefreshIcon />
              </IconButton>
            </Tooltip>
          </Stack>
        </Stack>
        <TableContainer className="mt-3 max-h-96 overflow-auto">
          <Table stickyHeader>
            <TableHead>
              <TableRow>
                <TableCell>Username</TableCell>
                <TableCell>First Name</TableCell>
                <TableCell>Last Name</TableCell>
                <TableCell>User Type</TableCell>
                <TableCell align="center">Profile</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {isLoaded
                ? userList
                    .filter(
                      (user) =>
                        searchString.length === 0 ||
                        [user.userName, user.email, `${user.firstName} ${user.lastName}`, user.id].some((prop) =>
                          prop.toLowerCase().includes(searchString.toLowerCase()),
                        ),
                    )
                    .map((user) => (
                      <TableRow key={user.id} hover>
                        <TableCell>{user.userName}</TableCell>
                        <TableCell>{user.firstName}</TableCell>
                        <TableCell>{user.lastName}</TableCell>
                        <TableCell>{user.userType}</TableCell>
                        <TableCell align="center">
                          <IconButton component={Link} to={`/profiles/${user.id}`}>
                            <AccountBoxIcon />
                          </IconButton>
                        </TableCell>
                      </TableRow>
                    ))
                : null}
            </TableBody>
          </Table>
        </TableContainer>
        {isLoaded ? null : (
          <Stack alignItems="center" className="mt-8">
            <CircularProgress />
          </Stack>
        )}
      </CardContent>
    </Card>
  );
};

export default UserListCard;
