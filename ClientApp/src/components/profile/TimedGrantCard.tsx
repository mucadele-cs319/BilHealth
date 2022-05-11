import React, { useState } from "react";
import { TimedAccessGrant } from "../../util/API/APITypes";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Typography from "@mui/material/Typography";
import TableContainer from "@mui/material/TableContainer";
import TableCell from "@mui/material/TableCell";
import TableHead from "@mui/material/TableHead";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableRow from "@mui/material/TableRow";
import Stack from "@mui/material/Stack";
import Button from "@mui/material/Button";
import TimedGrantRow from "./TimedGrantRow";

interface Props {
  refreshHandler: () => void;
  patientId: string;
  grants: TimedAccessGrant[];
}

const TimedGrantCard = ({ refreshHandler, patientId, grants }: Props) => {
  const [creating, setCreating] = useState(false);

  const refreshTimedGrants = () => {
    refreshHandler();
  };

  return (
    <Card className="max-w-screen-md mb-5 mx-auto">
      <CardContent>
        <Stack direction="row">
          <Typography variant="h5" gutterBottom>
            Timed Access Grants
            <Typography variant="body2">
              You can grant nurses timed access so that they are permitted to access your medical records.
            </Typography>
          </Typography>
          <Stack justifyContent="center" sx={{ flexGrow: 0, marginLeft: "auto" }}>
            <Button
              onClick={() => {
                setCreating(true);
              }}
              variant="text"
              disabled={creating}
            >
              Add
            </Button>
          </Stack>
        </Stack>

        <TableContainer className="mt-3 max-h-96 overflow-auto">
          <Table stickyHeader>
            <TableHead>
              <TableRow>
                <TableCell>Expiry</TableCell>
                <TableCell>User</TableCell>
                <TableCell align="right">Actions</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {creating ? (
                <TimedGrantRow
                  patientId={patientId}
                  cancelHandler={() => setCreating(false)}
                  refreshHandler={refreshTimedGrants}
                />
              ) : null}
              {grants?.map((grant) => (
                <TimedGrantRow key={grant.id} patientId={patientId} grant={grant} refreshHandler={refreshTimedGrants} />
              ))}
            </TableBody>
          </Table>
        </TableContainer>
        {grants?.length !== 0 ? null : (
          <Stack alignItems="center" className="mt-8">
            <Typography color="text.secondary">No grants added</Typography>
          </Stack>
        )}
      </CardContent>
    </Card>
  );
};

export default TimedGrantCard;
