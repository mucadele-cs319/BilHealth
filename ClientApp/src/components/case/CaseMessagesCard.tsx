import LoadingButton from "@mui/lab/LoadingButton";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Divider from "@mui/material/Divider";
import Stack from "@mui/material/Stack";
import TextField from "@mui/material/TextField";
import Typography from "@mui/material/Typography";
import React, { useState } from "react";
import APIClient from "../../util/API/APIClient";
import { Case, CaseMessage, CaseSystemMessage } from "../../util/API/APITypes";
import { isStaff } from "../../util/UserTypeUtil";
import { useUserContext } from "../UserContext";
import CaseMessageItem from "./CaseMessageItem";
import CaseSystemMessageItem from "./CaseSystemMessageItem";

interface Props {
  _case: Case;
  readonly: boolean;
  refreshHandler: () => void;
}

const CaseMessagesCard = ({ _case, readonly, refreshHandler }: Props) => {
  const { user } = useUserContext();

  const [isPending, setIsPending] = useState(false);
  const [newMessage, setNewMessage] = useState("");

  const handleNewMessage = async () => {
    setIsPending(true);
    await APIClient.cases.messages.add(_case.id, { content: newMessage });
    setIsPending(false);
    setNewMessage("");
    refreshHandler();
  };

  const validate = () => 0 < newMessage.length && newMessage.length < 2000;

  return (
    <Card className="max-w-screen-md mb-5 mx-auto">
      <CardContent>
        <Typography variant="h5" gutterBottom>
          Messages
        </Typography>

        {_case.messages.length > 0 || _case.systemMessages.length > 0 ? (
          [..._case.messages, ..._case.systemMessages]
            .sort((a, b) => (a.dateTime?.isAfter(b.dateTime) ? 1 : -1))
            .map((msg) => {
              const isSystemMessage = (msg as CaseMessage).userId === undefined;
              if (isSystemMessage) return <CaseSystemMessageItem message={msg as CaseSystemMessage} key={msg.id} />;
              else
                return (
                  <CaseMessageItem
                    readonly={(msg as CaseMessage).userId !== user?.id && !isStaff(user)}
                    message={msg as CaseMessage}
                    refreshHandler={refreshHandler}
                    key={msg.id}
                  />
                );
            })
        ) : (
          <Stack alignItems="center" className="my-8">
            <Typography color="text.secondary">No messages at this time.</Typography>
          </Stack>
        )}

        {readonly ? null : (
          <>
            <Divider />
            <Box>
              <TextField
                id="case-message-input"
                label="Message"
                variant="filled"
                multiline
                fullWidth
                margin="dense"
                minRows={4}
                value={newMessage}
                onChange={(e) => setNewMessage(e.target.value)}
              />
              <Stack direction="row" justifyContent="end">
                <Button variant="text" onClick={() => setNewMessage("")}>
                  Cancel
                </Button>
                <LoadingButton
                  loading={isPending}
                  disabled={!validate()}
                  onClick={handleNewMessage}
                  variant="text"
                  loadingPosition="center"
                >
                  Send
                </LoadingButton>
              </Stack>
            </Box>
          </>
        )}
      </CardContent>
    </Card>
  );
};

export default CaseMessagesCard;
