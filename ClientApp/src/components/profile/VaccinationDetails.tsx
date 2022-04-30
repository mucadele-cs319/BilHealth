import React, { useState } from "react";
import { Vaccination } from "../../util/API/APITypes";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Typography from "@mui/material/Typography";
import TableContainer from "@mui/material/TableContainer";
import TableCell from "@mui/material/TableCell";
import TableHead from "@mui/material/TableHead";
import Table from "@mui/material/Table";
import TableBody from "@mui/material/TableBody";
import TableRow from "@mui/material/TableRow";
import VaccinationRow from "./VaccinationRow";
import Stack from "@mui/material/Stack";
import Button from "@mui/material/Button";

interface Props {
  readonly?: boolean;
  refreshHandler: () => void;
  patientId: string;
  vaccinations?: Vaccination[];
}

const VaccinationDetails = ({ readonly = false, refreshHandler, patientId, vaccinations }: Props) => {
  const [creating, setCreating] = useState(false);

  const refreshVaccinations = () => {
    refreshHandler();
  };

  return (
    <Card className="max-w-screen-md mb-5 mx-auto">
      <CardContent>
        <Stack direction="row">
          <Typography variant="h5" gutterBottom>
            Vaccination Details
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

        <TableContainer className="mt-3 max-h-96 overflow-auto">
          <Table stickyHeader>
            <TableHead>
              <TableRow>
                <TableCell>Type</TableCell>
                <TableCell>Date</TableCell>
                {readonly ? null : <TableCell align="right">Actions</TableCell>}
              </TableRow>
            </TableHead>
            <TableBody>
              {creating ? (
                <VaccinationRow
                  patientId={patientId}
                  cancelHandler={() => setCreating(false)}
                  refreshHandler={refreshVaccinations}
                />
              ) : null}
              {vaccinations?.map((vaccination) => (
                <VaccinationRow
                  readonly={readonly}
                  key={vaccination.id}
                  patientId={patientId}
                  vaccination={vaccination}
                  refreshHandler={refreshVaccinations}
                />
              ))}
            </TableBody>
          </Table>
        </TableContainer>
        {vaccinations?.length !== 0 ? null : (
          <Stack alignItems="center" className="mt-8">
            <Typography color="text.secondary">No vaccinations added</Typography>
          </Stack>
        )}
      </CardContent>
    </Card>
  );
};

export default VaccinationDetails;
