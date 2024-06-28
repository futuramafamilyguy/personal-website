import React from "react";

import CapsuleButton from "../CapsuleButton/CapsuleButton";
import styles from "./Pagination.module.css";

interface PaginationProps {
  currentPage: number;
  totalPages: number;
  handlePrev: () => void;
  handleNext: () => void;
}

const Pagination: React.FC<PaginationProps> = ({
  currentPage,
  totalPages,
  handlePrev,
  handleNext,
}) => {
  return (
    <span className={styles.pagination}>
      <CapsuleButton
        text="Prev"
        onClick={handlePrev}
        disabled={currentPage === 1}
        selected={false}
      />
      <span className={styles.paginationSpan}>
        {currentPage}/{totalPages}
      </span>
      <CapsuleButton
        text="Next"
        onClick={handleNext}
        disabled={currentPage === totalPages}
        selected={false}
      />
    </span>
  );
};

export default Pagination;
