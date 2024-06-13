import { useEffect, useState } from "react";

import CapsuleButton from "../../components/CapsuleButton/CapsuleButton";
import MediaCard from "../../components/MediaCard/MediaCard";
import Pagination from "../../components/Pagination/Pagination";
import styles from "./LetterboxcPage.module.css";

function LetterboxcPage() {
  const handleButtonClick = () => {
    // placeholder handler
  };

  const pictures = [
    {
      imageUrl:
        "https://image.tmdb.org/t/p/original/a5lZ1NT9NvuUTvRwIJb0V6B43up.jpg",
      title: "Everybody Wants Some!!",
    },
    {
      imageUrl:
        "https://m.media-amazon.com/images/M/MV5BNWViNzU1MmEtYzJlMC00MjVlLTljNzctMzVkNjRjZGVhNjg4XkEyXkFqcGdeQVRoaXJkUGFydHlJbmdlc3Rpb25Xb3JrZmxvdw@@._V1_.jpg",
      title: "Barbarian",
    },
    {
      imageUrl:
        "https://beforesandafters.com/wp-content/uploads/2023/10/Spider-Man-India-Pavitr-Prabhakar-Spider-Man-Across-the-Spider-Verse.webp",
      title: "Across the Spiderverse",
    },
    {
      imageUrl:
        "https://variety.com/wp-content/uploads/2024/04/The-First-Omen.jpg?w=1000",
      title: "The First Omen",
    },
    {
      imageUrl: "https://variety.com/wp-content/uploads/2021/07/Red-Rocket.jpg",
      title: "Red Rocket",
    },
    {
      imageUrl:
        "https://scrapsfromtheloft.com/wp-content/uploads/2023/11/The-Holdovers-2023.jpg",
      title: "The Holdovers",
    },
    {
      imageUrl:
        "https://image.tmdb.org/t/p/original/a6SnT3uD5kCrCNkNwng8FmXJXZf.jpg",
      title: "Pearl",
    },
  ];

  // --- pagination logic start ---
  const [currentPage, setCurrentPage] = useState(1);
  const [itemsPerPage, setItemsPerPage] = useState(1);

  const calculateItemsPerPage = () => {
    const containerWidth =
      document.querySelector(`.${styles.contentArea}`)?.clientWidth || 0;
    const itemWidth = 235; // media card width + horizontal padding
    const columns = Math.floor(containerWidth / itemWidth);

    const containerHeight =
      document.querySelector(`.${styles.contentArea}`)?.clientHeight || 0;
    const itemHeight = 150; // media card height + vertical padding
    const rows = Math.floor(containerHeight / itemHeight);

    return columns * rows;
  };

  useEffect(() => {
    const updateItemsPerPage = () => setItemsPerPage(calculateItemsPerPage());
    updateItemsPerPage();
    window.addEventListener("resize", updateItemsPerPage);

    return () => window.removeEventListener("resize", updateItemsPerPage);
  }, []);

  const totalPages = Math.ceil(pictures.length / itemsPerPage);

  const currentPictures = pictures.slice(
    (currentPage - 1) * itemsPerPage,
    currentPage * itemsPerPage
  );

  // fill any empty spaces in the flex content area with empty media cards so the positioning isnt scuffed AF
  const emptyMediaCardArray = Array.from(
    { length: itemsPerPage - currentPictures.length },
    (_, index) => index + 1
  );
  // --- pagination logic end ---

  return (
    <>
      <div className={styles.letterboxcPage}>
        <div className={styles.descriptionBox}>
          <h4>Letterboxc (c to avoid copyright)</h4>
          <br />
          <p>Pictures I've seen at the cinemas over the past few years.</p>
        </div>
        <div className={styles.activeYearsArea}>
          <CapsuleButton
            text="Favorite of the Year"
            onClick={handleButtonClick}
            disabled={false}
          />
        </div>

        <div className={styles.contentArea}>
          {currentPictures.map((picture, index) => (
            <MediaCard imageUrl={picture.imageUrl} title={picture.title} />
          ))}
          {currentPage === totalPages
            ? emptyMediaCardArray.map(() => (
                <div className={styles.emptyMediaCard}></div>
              ))
            : null}
        </div>
        <Pagination
          currentPage={currentPage}
          totalPages={totalPages}
          onPageChange={setCurrentPage}
        />
      </div>
    </>
  );
}

export default LetterboxcPage;
