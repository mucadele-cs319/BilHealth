import LoadingButton from "@mui/lab/LoadingButton";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import React, { useState } from "react";
import APIClient from "../../util/API/APIClient";
import { Case } from "../../util/API/APITypes";
import AppointmentItemCard from "./AppointmentItemCard";

interface Props {
  _case: Case;
  refreshHandler: () => void;
}

const AppointmentCard = ({ _case, refreshHandler }: Props) => {
  return (
    <Card className="max-w-screen-md mb-5 mx-auto">
      <CardContent>
        <Typography variant="h5" gutterBottom>
          Appointments
        </Typography>
        {_case.appointments.map((apt) => {
          <AppointmentItemCard _case={_case} appointment={apt} key={apt.id} refreshHandler={refreshHandler} />;
        })}
      </CardContent>
    </Card>
  );
};

export default AppointmentCard;
