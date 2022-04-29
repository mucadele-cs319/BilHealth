import React from "react";
import Box from "@mui/material/Box";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Chip from "@mui/material/Chip";
import Grid from "@mui/material/Grid";
import Typography from "@mui/material/Typography";
import { stringifyBloodType, stringifyCampus, stringifyGender, User, UserType } from "../../util/API/APITypes";
import ProfileDetailsItem from "./ProfileDetailsItem";

interface Props {
  data: User;
}

const failsafe = (content: string | number | undefined) => {
  if (typeof content === "number") return content.toString();
  return content || "—";
};

const ProfileDetails = ({ data }: Props) => {
  return (
    <Card className="max-w-screen-md mb-5 mx-auto">
      <CardContent>
        <Typography variant="h5" gutterBottom>
          {`${data.firstName} ${data.lastName}`} <Chip className="ml-2" size="small" label={data.userType} />
        </Typography>

        <Grid container spacing={2} mt={1}>
          <Grid item>
            <ProfileDetailsItem title="Email" content={failsafe(data.email)} />
          </Grid>
          <Grid item>
            <ProfileDetailsItem title="Gender" content={stringifyGender(data.gender)} />
          </Grid>
          <Grid item>
            <ProfileDetailsItem title="Date of Birth" content={failsafe(data.dateOfBirth?.toString())} />
          </Grid>
        </Grid>

        {data.userType !== UserType.Patient ? null : (
          <Box mt={2}>
            <Typography variant="subtitle1">Patient Details</Typography>
            <Grid container spacing={2}>
              <Grid item>
                <ProfileDetailsItem title="Body Weight" content={failsafe(data.bodyWeight)} />
              </Grid>
              <Grid item>
                <ProfileDetailsItem title="Body Height" content={failsafe(data.bodyHeight)} />
              </Grid>
              <Grid item>
                <ProfileDetailsItem title="Blood Type" content={stringifyBloodType(data.bloodType)} />
              </Grid>
              <Grid item>
                <ProfileDetailsItem title="Blacklisted" content={data.blacklisted ? "Yes" : "No"} />
              </Grid>
            </Grid>
          </Box>
        )}

        {data.userType !== UserType.Doctor ? null : (
          <Box mt={2}>
            <Typography variant="subtitle1">Doctor Details</Typography>
            <Grid container spacing={2}>
              <Grid item>
                <ProfileDetailsItem title="Specialization" content={failsafe(data.specialization)} />
              </Grid>
              <Grid item>
                <ProfileDetailsItem title="Campus" content={stringifyCampus(data.campus)} />
              </Grid>
            </Grid>
          </Box>
        )}
      </CardContent>
    </Card>
  );
};

export default ProfileDetails;
