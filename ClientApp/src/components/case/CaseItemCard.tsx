import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Chip from "@mui/material/Chip";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import React from "react";
import { useNavigate } from "react-router-dom";
import { SimpleCase, stringifyCaseState, stringifyCaseType } from "../../util/API/APITypes";

interface Props {
  _case: SimpleCase;
}

const CaseItemCard = ({ _case }: Props) => {
  const navigate = useNavigate();

  return (
    <Card className="max-w-screen-md mb-5 mx-auto">
      <CardContent>
        <Stack direction="row">
          <Box>
            <Typography variant="h6" gutterBottom>
              {_case.title}
              <Chip className="ml-2" size="small" label={stringifyCaseState(_case.state)} />
              <Chip className="ml-2" size="small" label={stringifyCaseType(_case.type)} />
            </Typography>
            <Typography variant="body2" color="text.secondary">
              Opened on {_case.dateTime.format("DD/MM/YYYY, HH:mm")} by{" "}
              {`${_case.simplePatientUser?.firstName} ${_case.simplePatientUser?.lastName}`}
            </Typography>
          </Box>
          <Stack justifyContent="center" sx={{ flexGrow: 0, marginLeft: "auto" }}>
            <Button onClick={() => navigate(`/cases/${_case.id}`)} variant="text">
              View Case
            </Button>
          </Stack>
        </Stack>
        <Typography variant="body2">Message Count: {_case.messageCount}</Typography>
        <Typography variant="body2">
          Doctor Assigned:{" "}
          {_case.doctorUserId === null
            ? "N/A"
            : `${_case.simpleDoctorUser?.firstName} ${_case.simpleDoctorUser?.lastName}`}
        </Typography>

        <Stack direction="row" justifyContent="end"></Stack>
      </CardContent>
    </Card>
  );
};

export default CaseItemCard;