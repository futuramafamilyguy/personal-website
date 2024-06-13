import React from "react";

import styles from "./CapsuleButton.module.css";

interface CapsuleButtonProps {
  text: string;
  onClick: () => void;
  disabled: boolean;
}

const CapsuleButton: React.FC<CapsuleButtonProps> = ({
  text,
  onClick,
  disabled,
}) => {
  return (
    <button className={styles.button} onClick={onClick} disabled={disabled}>
      {text}
    </button>
  );
};

export default CapsuleButton;
