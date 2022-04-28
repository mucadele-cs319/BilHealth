import React from "react";
import { useDocumentTitle } from "../../util/CustomHooks";

const CalculatorsPage = () => {
  useDocumentTitle("Calculators");

  return <div id="calculators-container"></div>;
};

export default CalculatorsPage;
