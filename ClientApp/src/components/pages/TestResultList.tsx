import React from "react";
import { useDocumentTitle } from "../../util/CustomHooks";

const TestResultList = () => {
  useDocumentTitle("Test Results");

  return <div id="testresult-list-container"></div>;
};

export default TestResultList;
