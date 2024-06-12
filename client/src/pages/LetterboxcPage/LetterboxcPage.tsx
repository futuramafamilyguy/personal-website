import styles from "./LetterboxcPage.module.css";

function LetterboxcPage() {
  return (
    <>
      <div className={styles.letterboxcPage}>
        <div className={styles.descriptionBox}>
          <h3>Letterboxc (c to avoid copyright)</h3>
          <br />
          <p>
            Picutres I've seen at the cinemas over the past few years. I'm
            NZ-based so some pictures aren't available to us until much later
            than other countires. Like even though Pearl (2022) was filmed in
            NZ, we only got a SINGLE screening of it 6 months after it was
            released in the US. bruh
          </p>
        </div>
      </div>
    </>
  );
}

export default LetterboxcPage;
