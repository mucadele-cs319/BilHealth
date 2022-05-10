import LoadingButton from "@mui/lab/LoadingButton";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import React, { useState } from "react";
import APIClient from "../../util/API/APIClient";
import { Case } from "../../util/API/APITypes";

interface Props {
  _case: Case;
  refreshHandler: () => void;
}

const AppointmentCard = ({ _case }: Props) => {


  return (
    <Card className="max-w-screen-md mb-5 mx-auto">
      <CardContent>
        <Typography variant="h5" gutterBottom>
          Appointments
        </Typography>



      </CardContent>
    </Card>
  );
};

export default AppointmentCard;
