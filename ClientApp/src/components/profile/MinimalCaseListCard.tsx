import React from "react";
import Stack from "@mui/material/Stack";
import Typography from "@mui/material/Typography";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import { SimpleCase } from "../../util/API/APITypes";
import MinimalCaseItemCard from "./MinimalCaseItemCard";

interface Props {
  cases: SimpleCase[];
}

const MinimalCaseListCard = ({ cases }: Props) => {
  return (
    <Card className="max-w-screen-md mb-5 mx-auto">
      <CardContent>
        <Stack direction="row">
          <Typography variant="h5" gutterBottom>
            Cases
          </Typography>
        </Stack>

        {cases.length > 0 ? (
          cases.map((_case) => <MinimalCaseItemCard key={_case.id} _case={_case} />)
        ) : (
          <Stack alignItems="center" className="mt-8">
            <Typography color="text.secondary">No viewable cases for you at this time.</Typography>
          </Stack>
        )}
      </CardContent>
    </Card>
  );
};

export default MinimalCaseListCard;
