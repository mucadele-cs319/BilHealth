import React from "react";
import { Announcement } from "../util/API/APITypes";

interface Props {
  data: Announcement;
}

const AnnouncementItem = ({ data }: Props): JSX.Element => {
  return (
    <article>
      <h3>{data?.title}</h3>
      <p>{data?.message}</p>
    </article>
  );
};

export default AnnouncementItem;
