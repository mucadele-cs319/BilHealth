import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import React from "react";
import { Link } from "react-router-dom";
import BiotechIcon from "@mui/icons-material/Biotech";
import Button from "@mui/material/Button";

const TestResultLinkerCard = () => {
  return (
    <Card className="max-w-screen-md mb-5 mx-auto">
      <CardContent>
        <Typography variant="h5" gutterBottom>
          Test Results <BiotechIcon />
        </Typography>
        <Typography variant="body2">Access medical test results online through BilHealth.</Typography>

        <Stack direction="row" justifyContent="end">
          <Button component={Link} to="test-results" variant="text">
            View Test Results
          </Button>
        </Stack>
      </CardContent>
    </Card>
  );
};

export default TestResultLinkerCard;
