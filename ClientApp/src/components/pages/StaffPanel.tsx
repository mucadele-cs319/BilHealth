import React from "react";
import { useDocumentTitle } from "../../util/CustomHooks";

const StaffPanel = () => {
  useDocumentTitle("Staff Panel");

  return <div id="staffpanel-container"></div>;
};

export default StaffPanel;
