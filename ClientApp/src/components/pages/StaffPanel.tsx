import React from "react";
import { useDocumentTitle } from "../../util/CustomHooks";
import Fade from "@mui/material/Fade";
import Grid from "@mui/material/Grid";
import RegistrationCard from "../staffpanel/RegistrationCard";
import UserListCard from "../staffpanel/UserListCard";

const StaffPanel = () => {
  useDocumentTitle("Staff Panel");

  return (
    <Grid container justifyContent="center">
      <Fade in={true}>
        <Grid item lg={10} xs={11}>
          <RegistrationCard />
          <UserListCard />
        </Grid>
      </Fade>
    </Grid>
  );
};

export default StaffPanel;
