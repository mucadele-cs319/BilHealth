import React from "react";
import { useDocumentTitle } from "../../util/CustomHooks";

const CasePage = () => {
  useDocumentTitle(`Case #${123}`);

  return <div id="case-container"></div>;
};

export default CasePage;
