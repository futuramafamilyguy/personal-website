import axios from "axios";
import { useEffect, useState } from "react";

import { Picture } from "../types/Picture";

const usePictures = (initialPage: number) => {
  const calculateItemsPerPage = () => {
    // using hard-coded values here since flex box dimensions are inconsistent
    // remember to change the width and height offset if any other elements change lol
    const containerWidth = window.innerWidth - 57;
    const itemWidth = 235; // media card width + horizontal padding
    const columns = Math.floor(containerWidth / itemWidth);
    const containerHeight = window.innerHeight - 280;
    const itemHeight = 150; // media card height + vertical padding
    const rows = Math.floor(containerHeight / itemHeight);

    return columns * rows;
  };

  const [pictures, setPictures] = useState<Picture[]>([]);
  const [year, setYear] = useState(() => {
    let currentDate = new Date();
    let currentYear = currentDate.getFullYear();
    return currentYear;
  });
  const [currentPage, setCurrentPage] = useState(initialPage);
  const [totalPages, setTotalPages] = useState(0);
  const [itemsPerPage, setItemsPerPage] = useState(() =>
    calculateItemsPerPage()
  );
  const [loading, setLoading] = useState(false);

  // fill any empty spaces in the flex content area with empty media cards so the positioning isnt scuffed AF
  const emptyMediaCardArray = Array.from(
    { length: itemsPerPage - pictures.length },
    (_, index) => index + 1
  );

  useEffect(() => {
    const updateItemsPerPage = () => setItemsPerPage(calculateItemsPerPage());
    updateItemsPerPage();
    window.addEventListener("resize", updateItemsPerPage);

    return () => window.removeEventListener("resize", updateItemsPerPage);
  }, []);

  useEffect(() => {
    setCurrentPage(1);
  }, [year, totalPages]);

  useEffect(() => {
    const fetchPictures = async () => {
      setLoading(true);
      try {
        const response = await axios(
          `https://localhost:7044/pictures/${year}?pageNumber=${currentPage}&pageSize=${itemsPerPage}`
        );

        const data = response.data;
        setPictures(data.pictures);
        setTotalPages(Math.ceil(data.totalCount / itemsPerPage));
      } catch (error) {
        console.error("Failed to fetch pictures", error);
      } finally {
        setLoading(false);
      }
    };

    fetchPictures();
  }, [year, currentPage, itemsPerPage]);

  return {
    pictures,
    currentPage,
    totalPages,
    loading,
    year,
    emptyMediaCardArray,
    setCurrentPage,
    setYear,
  };
};

export default usePictures;
