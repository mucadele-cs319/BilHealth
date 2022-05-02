import Button from "@mui/material/Button";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import CircularProgress from "@mui/material/CircularProgress";
import Fade from "@mui/material/Fade";
import Grid from "@mui/material/Grid";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import React, { useEffect, useState } from "react";
import { useParams } from "react-router-dom";
import APIClient from "../../util/API/APIClient";
import { User } from "../../util/API/APITypes";
import { titleify, useDocumentTitle } from "../../util/CustomHooks";
import { isStaff } from "../../util/UserTypeUtil";
import TestResultRow from "../profile/TestResultRow";
import { useUserContext } from "../UserContext";
import TableContainer from "@mui/material/TableContainer";
import TableCell from "@mui/material/TableCell";
import TableHead from "@mui/material/TableHead";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableRow from "@mui/material/TableRow";

const TestResultList = () => {
  useDocumentTitle("Test Results");

  const { user } = useUserContext();
  const readonly = !isStaff(user);

  const params = useParams();
  const [queryUser, setQueryUser] = useState<User>();
  const [isLoaded, setIsLoaded] = useState(false);

  const [creating, setCreating] = useState(false);

  const refreshUser = () => {
    if (params.userid === undefined) throw Error("Couldn't get URL param for user ID");
    APIClient.profiles.get(params.userid).then((fetchedUser) => {
      setQueryUser(fetchedUser);
      document.title = titleify(`${fetchedUser.firstName}'s Test Results`);
      setIsLoaded(true);
    });
  };

  useEffect(() => {
    refreshUser();
  }, [params]);

  return (
    <Grid container justifyContent="center">
      <Fade in={true}>
        <Grid item lg={10} xs={11}>
          {isLoaded && queryUser ? (
            <Card className="max-w-screen-md mb-5 mx-auto">
              <CardContent>
                <Stack direction="row" justifyContent="center">
                  <Typography variant="h5" gutterBottom>
                    {`${queryUser.firstName} ${queryUser.lastName}'s Test Results`}
                  </Typography>
                  <Stack justifyContent="center" sx={{ flexGrow: 0, marginLeft: "auto" }}>
                    {readonly ? null : (
                      <Button
                        onClick={() => {
                          setCreating(true);
                        }}
                        variant="text"
                        disabled={creating}
                      >
                        Add
                      </Button>
                    )}
                  </Stack>
                </Stack>

                <TableContainer className="mt-3">
                  <Table stickyHeader>
                    <TableHead>
                      <TableRow>
                        <TableCell>Type</TableCell>
                        <TableCell>Date</TableCell>
                        <TableCell align="right">Actions</TableCell>
                      </TableRow>
                    </TableHead>
                    <TableBody>
                      {creating ? (
                        <TestResultRow
                          patientId={queryUser.id || ""}
                          cancelHandler={() => setCreating(false)}
                          refreshHandler={refreshUser}
                        />
                      ) : null}
                      {queryUser.testResults?.map((vaccination) => (
                        <TestResultRow
                          readonly={readonly}
                          key={vaccination.id}
                          patientId={queryUser.id || ""}
                          testResult={vaccination}
                          refreshHandler={refreshUser}
                        />
                      ))}
                    </TableBody>
                  </Table>
                </TableContainer>
                {queryUser.testResults?.length !== 0 ? null : (
                  <Stack alignItems="center" className="mt-8">
                    <Typography color="text.secondary">No test results added</Typography>
                  </Stack>
                )}
              </CardContent>
            </Card>
          ) : (
            <Stack alignItems="center" className="mt-8">
              <CircularProgress />
            </Stack>
          )}
        </Grid>
      </Fade>
    </Grid>
  );
};

export default TestResultList;
