import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Chip from "@mui/material/Chip";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import React from "react";
import { Link } from "react-router-dom";
import { SimpleCase, stringifyCaseState, stringifyCaseType } from "../../util/API/APITypes";
import { fmtConcise } from "../../util/StringUtil";

interface Props {
  _case: SimpleCase;
}

const MinimalCaseItemCard = ({ _case }: Props) => {
  return (
    <Card className="max-w-screen-md mb-1 mx-auto">
      <CardContent>
        <Stack direction="row">
          <Box>
            <Typography variant="h6" fontSize="medium" gutterBottom>
              {_case.title}
              <Chip className="ml-2" size="small" label={stringifyCaseState(_case.state)} />
              <Chip className="ml-2" size="small" label={stringifyCaseType(_case.type)} />
            </Typography>
            <Typography variant="body2" color="text.secondary">
              Opened on {_case.dateTime.format(fmtConcise)}
            </Typography>
          </Box>
          <Stack justifyContent="center" sx={{ flexGrow: 0, marginLeft: "auto" }}>
            <Button component={Link} to={`/cases/${_case.id}`} variant="text">
              View Case
            </Button>
          </Stack>
        </Stack>
      </CardContent>
    </Card>
  );
};

export default MinimalCaseItemCard;
