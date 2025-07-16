import React from "react";

import styles from "./CapsuleButton.module.css";

interface CapsuleButtonProps {
  text: string;
  onClick: () => void;
  disabled: boolean;
  selected: boolean;
  width?: string | number;
}

const CapsuleButton: React.FC<CapsuleButtonProps> = ({
  text,
  onClick,
  disabled,
  selected,
  width,
}) => {
  return (
    <button
      className={`${styles.button} ${selected ? styles.selected : ""}`}
      onClick={onClick}
      disabled={disabled}
      style={{ width }}
    >
      {text}
    </button>
  );
};

export default CapsuleButton;
