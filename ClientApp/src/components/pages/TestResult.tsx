import React from "react";
import { useDocumentTitle } from "../../util/CustomHooks";

const TestResultPage = () => {
  useDocumentTitle("Test Result");

  return <div id="testresult-container"></div>;
};

export default TestResultPage;
