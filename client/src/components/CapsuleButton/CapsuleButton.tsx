import React from "react";

import styles from "./CapsuleButton.module.css";

interface CapsuleButtonProps {
  text: string;
  onClick: () => void;
  disabled: boolean;
  selected: boolean;
}

const CapsuleButton: React.FC<CapsuleButtonProps> = ({
  text,
  onClick,
  disabled,
  selected,
}) => {
  return (
    <button
      className={`${styles.button} ${selected ? styles.selected : ""}`}
      onClick={onClick}
      disabled={disabled}
    >
      {text}
    </button>
  );
};

export default CapsuleButton;
