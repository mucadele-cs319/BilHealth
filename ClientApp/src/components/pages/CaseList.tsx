import React from "react";
import { useDocumentTitle } from "../../util/CustomHooks";

const CaseList = () => {
  useDocumentTitle("Cases");

  return <div id="caselist-container"></div>;
};

export default CaseList;
