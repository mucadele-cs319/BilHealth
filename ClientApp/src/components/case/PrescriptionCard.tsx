import { Case } from "../../util/API/APITypes";
import React, { useState } from "react";
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
import PrescriptionCardRow from "./PrescriptionCardRow";

interface Props {
  _case: Case;
  readonly?: boolean;
  refreshHandler: () => void;
}

const PrescriptionCard = ({ _case, refreshHandler, readonly = false }: Props) => {
  const [creating, setCreating] = useState(false);

  const refreshPrescriptions = () => {
    refreshHandler();
  };

  return (
    <Card className="max-w-screen-md mb-5 mx-auto">
      <CardContent>
        <Stack direction="row">
          <Typography variant="h5" gutterBottom>
            Prescriptions
          </Typography>
          <Stack justifyContent="center" sx={{ flexGrow: 0, marginLeft: "auto" }}>
            <Button
              onClick={() => {
                setCreating(true);
              }}
              variant="text"
              disabled={creating || readonly}
            >
              Add
            </Button>
          </Stack>
        </Stack>

        <TableContainer className="mt-3 max-h-96 overflow-auto">
          <Table stickyHeader>
            <TableHead>
              <TableRow>
                <TableCell>Date</TableCell>
                <TableCell>Items</TableCell>
                <TableCell align="right">Actions</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {creating ? (
                <PrescriptionCardRow
                  _case={_case}
                  cancelHandler={() => setCreating(false)}
                  refreshHandler={refreshPrescriptions}
                />
              ) : null}
              {_case.prescriptions?.map((prescription) => (
                <PrescriptionCardRow
                  readonly={readonly}
                  key={prescription.id}
                  _case={_case}
                  prescription={prescription}
                  refreshHandler={refreshPrescriptions}
                />
              ))}
            </TableBody>
          </Table>
        </TableContainer>
        {_case.prescriptions?.length !== 0 ? null : (
          <Stack alignItems="center" className="mt-8">
            <Typography color="text.secondary">No prescriptions added</Typography>
          </Stack>
        )}
      </CardContent>
    </Card>
  );
};

export default PrescriptionCard;
