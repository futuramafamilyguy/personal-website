import React from "react";

import CapsuleButton from "../CapsuleButton/CapsuleButton";
import styles from "./Pagination.module.css";

interface PaginationProps {
  currentPage: number;
  totalPages: number;
  onPageChange: (page: number) => void;
}

const Pagination: React.FC<PaginationProps> = ({
  currentPage,
  totalPages,
  onPageChange,
}) => {
  const handlePrevClick = () => {
    if (currentPage > 1) {
      onPageChange(Math.max(currentPage - 1, 1));
    }
  };

  const handleNextClick = () => {
    if (currentPage < totalPages) {
      onPageChange(Math.min(currentPage + 1, totalPages));
    }
  };

  return (
    <span className={styles.pagination}>
      <CapsuleButton
        text="Prev"
        onClick={handlePrevClick}
        disabled={currentPage === 1}
        selected={false}
      />
      <span className={styles.paginationSpan}>
        {currentPage}/{totalPages}
      </span>
      <CapsuleButton
        text="Next"
        onClick={handleNextClick}
        disabled={currentPage === totalPages}
        selected={false}
      />
    </span>
  );
};

export default Pagination;
