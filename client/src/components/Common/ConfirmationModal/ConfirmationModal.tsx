import styles from "./ConfirmationModal.module.css";

interface ConfirmationModalProps {
  text: string;
  onConfirm: () => void;
  onCancel: () => void;
  onClose: () => void;
}

const ConfirmationModal: React.FC<ConfirmationModalProps> = ({
  text,
  onConfirm,
  onCancel,
  onClose,
}) => {
  return (
    <div className={styles.overlay} onClick={onClose}>
      <div className={styles.modal} onClick={(e) => e.stopPropagation()}>
        <div className={`${styles.textContainer} bg-dark`}>
          <p>{text}</p>
          <div className={styles.buttonContainer}>
            <button className={`${styles.button} bg-dark`} onClick={onConfirm}>
              Yes
            </button>
            <button className={`${styles.button} bg-dark`} onClick={onCancel}>
              No
            </button>
          </div>
        </div>
      </div>
    </div>
  );
};

export default ConfirmationModal;
