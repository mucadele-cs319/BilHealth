import React, { useEffect, useState } from "react";
import Stack from "@mui/material/Stack";
import CircularProgress from "@mui/material/CircularProgress";
import { AuditTrail } from "../../util/API/APITypes";
import APIClient from "../../util/API/APIClient";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import TableContainer from "@mui/material/TableContainer";
import TableCell from "@mui/material/TableCell";
import TableHead from "@mui/material/TableHead";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableRow from "@mui/material/TableRow";
import Typography from "@mui/material/Typography";
import { fullNameify } from "../../util/StringUtil";

const AuditTrailCard = () => {
  const [trailList, setTrailList] = useState<AuditTrail[]>([]);
  const [isLoaded, setIsLoaded] = useState(false);

  const refreshTrailList = () => {
    setIsLoaded(false);
    APIClient.profiles.accessControl.getAuditTrails().then((auditTrails) => {
      setTrailList(auditTrails);
      setIsLoaded(true);
    });
  };

  useEffect(() => {
    refreshTrailList();
  }, []);

  return (
    <Card className="max-w-screen-md mb-5 mx-auto">
      <CardContent>
        <Typography variant="h5" gutterBottom sx={{ mb: 3 }}>
          Audit Trails
          <Typography variant="body2">An audit trail allows keeping a record of who visited whose profile.</Typography>
        </Typography>

        <TableContainer className="mt-3 max-h-96 overflow-auto">
          <Table stickyHeader>
            <TableHead>
              <TableRow>
                <TableCell>Date</TableCell>
                <TableCell>Accessed User</TableCell>
                <TableCell>Accessing User</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {isLoaded
                ? trailList.map((trail) => (
                    <TableRow key={trail.id} hover>
                      <TableCell>{trail.accessTime.format("DD/MM/YYYY, HH:mm")}</TableCell>
                      <TableCell>{`${fullNameify(trail.accessedUser)} (${trail.accessedUser.userName})`}</TableCell>
                      <TableCell>{`${fullNameify(trail.accessingUser)} (${trail.accessingUser.userName})`}</TableCell>
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

export default AuditTrailCard;
