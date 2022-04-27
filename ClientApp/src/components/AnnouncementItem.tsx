import React, { useState } from "react";
import { Announcement } from "../util/API/APITypes";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Typography from "@mui/material/Typography";
import AnnouncementItemEditable from "./AnnouncementItemEditable";

interface Props {
  data: Announcement;
  className?: string;
  changeHandler: () => void;
}

const AnnouncementItem = ({ data, className, changeHandler }: Props) => {
  const [editing, setEditing] = useState(false);

  return (
    <>
      {editing ? (
        <AnnouncementItemEditable changeHandler={changeHandler} />
      ) : (
        <Card className={`max-w-screen-md ${className}`}>
          <CardContent>
            <Typography variant="h5">{data.title}</Typography>
            <Typography variant="subtitle2" color="text.secondary" gutterBottom>
              {data.dateTime?.toString()}
            </Typography>
            <Typography variant="body1">{data.message}</Typography>
          </CardContent>
        </Card>
      )}
    </>
  );
};

export default AnnouncementItem;
