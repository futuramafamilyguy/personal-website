import styles from "./MessageDisplay.module.css";

interface MessageDisplayProps {
  message: string;
  imageUrl?: string;
}

const MessageDisplay: React.FC<MessageDisplayProps> = ({
  message,
  imageUrl,
}) => {
  return (
    <div className={styles.textArea}>
      {imageUrl !== null && <img className={styles.image} src={imageUrl} />}

      <h4>{message}</h4>
    </div>
  );
};

export default MessageDisplay;
